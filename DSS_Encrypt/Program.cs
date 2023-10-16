﻿using System.Net.Security;
using System.Numerics;
using System.Text;

//Variables a utilizar
ulong P = 17, Q = 19, id = 000001;
List<ulong> S = new();
S.Add(5);
int N = 10;
string? message = "";
string PSN = "    ";
List<ulong> keys = new();



//Directorio de mensajes
var location = AppDomain.CurrentDomain.BaseDirectory;
string messageDir = Directory.GetParent(location).Parent.Parent.Parent.Parent.FullName + "\\Messages\\";

Console.WriteLine("Bienvenido al cifrador de mensajes.\n");

char opt;
int i = 0;

while (true)
{
    Console.Clear();
    do
    {
        if (i > 0) Console.WriteLine("ERROR: Opcion no valida, intente de nuevo.\n");
        Console.WriteLine("¿Que tipo de mensaje desea enviar?\n");
        Console.WriteLine("1. Conectar con el receptor");
        Console.WriteLine("2. Enviar mensaje");
        Console.WriteLine("3. Actualizar llaves");
        Console.WriteLine("3. Desconectar del receptor");
        Console.WriteLine("0. Salir\n");
        opt = Console.ReadKey().KeyChar;
        i++;
    }
    while (opt != '0' && opt != '1' && opt != '2' && opt != '3');
    i = 0; 

    switch (opt)
    {
        case '1':
            Console.Clear();
            Console.WriteLine("TIPO DE MENSAJE: FCM\n");
            i++;
            while (i > 0)
            {
                try
                {
                    i--;
                    Console.WriteLine("Ingrese el numero de llaves a utilizar:");
                    N = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    i++;
                    Console.WriteLine("ERROR: El numero de llaves debe ser un numero entero positivo, intente de nuevo.\n");
                }
                if (N <= 0)
                {
                    i++;
                    Console.WriteLine("ERROR: El numero de llaves debe ser un numero entero positivo, intente de nuevo.\n");
                }
            }
            FCM(N);
            break;
        case '2':
            do
            {
                if (i > 0) Console.WriteLine("El mensaje no puede estar vacio, intente de nuevo.\n");
                Console.WriteLine("Ingrese el mensaje a enviar:");
                message = Console.ReadLine();
                i++;
            }
            while (string.IsNullOrWhiteSpace(message));
            break;
        case '3':
            Console.Clear();
            Console.WriteLine("TIPO DE MENSAJE: KU\n");
            i++;
            while (i > 0)
            {
                try
                {
                    i--;
                    Console.WriteLine("Ingrese el NUEVO NUMERO de llaves a utilizar:");
                    N = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    i++;
                    Console.WriteLine("ERROR: El numero de llaves debe ser un numero entero positivo, intente de nuevo.\n");
                }
                if (N <= 0)
                {
                    i++;
                    Console.WriteLine("ERROR: El numero de llaves debe ser un numero entero positivo, intente de nuevo.\n");
                }
            }
            KU(N);
            break;
        case '0':
            Environment.Exit(0);
            break;
        default:

            break;
    }
}

void FCM(int keyValue)
{
    //Restaura para iniciar de nuevo
    var tempS = S.First();
    S = new();
    S.Add(tempS);

    message = P + "." + Q + "." + S.First() + "." + keyValue; //Genera el mensaje FCM
    PSN = "    "; //Genera el PSN

    //Convierte los datos en Bytes
    byte[] idBytes = BitConverter.GetBytes(id);
    byte[] typeBytes = Encoding.UTF8.GetBytes("FCM ");
    List<byte[]> messageBytes = messageConverter(message);
    byte[] psnBytes = Encoding.UTF8.GetBytes(PSN);

    //Enlaza todos los bytes en un solo byte array
    byte[] output = new byte[idBytes.Length + typeBytes.Length + messageBytes.Sum(arr => arr.Length) + psnBytes.Length];
    Buffer.BlockCopy(idBytes, 0, output, 0, idBytes.Length);
    Buffer.BlockCopy(typeBytes, 0, output, idBytes.Length, typeBytes.Length);
    Buffer.BlockCopy(messageBytes.SelectMany(arr => arr).ToArray(), 0, output, idBytes.Length + typeBytes.Length, messageBytes.Sum(arr => arr.Length));
    Buffer.BlockCopy(psnBytes, 0, output, idBytes.Length + typeBytes.Length + messageBytes.Sum(arr => arr.Length), psnBytes.Length);    
    
    //Crea y escribe el mensaje en el archivo
    DirectoryInfo mDir = new(messageDir);
    FileInfo[] files = mDir.GetFiles();
    foreach (var file in files) file.Delete();
    File.Create(messageDir + "\\message.txt").Close();
    File.WriteAllBytes(messageDir + "\\message.txt", output);


    //Crea las llaves
    KeyCreator(keyValue);

    Console.WriteLine("Mensaje enviado. ID del mensaje: " + id);
    Console.WriteLine("Tipo de mensaje: FCM.\nLlaves creadas correctamente.");
    Console.WriteLine("Presione cualquier tecla para continuar...");
    Console.ReadKey();

    //Prepara los datos para la siguiente iteracion
    id++;
}

void KU (int keyValue)
{
    //Restaura para iniciar de nuevo
    var tempS = S.First();
    S = new();
    S.Add(tempS);
    keys = new(); //ELIMINA LAS LLAVES PARA RECREARLAS

    message = P + "." + Q + "." + S.First() + "." + keyValue; //Genera el mensaje FCM
    PSN = "    "; //Genera el PSN

    //Convierte los datos en Bytes
    byte[] idBytes = BitConverter.GetBytes(id);
    byte[] typeBytes = Encoding.UTF8.GetBytes("KU  ");
    List<byte[]> messageBytes = messageConverter(message);
    byte[] psnBytes = Encoding.UTF8.GetBytes(PSN);

    //Enlaza todos los bytes en un solo byte array
    byte[] output = new byte[idBytes.Length + typeBytes.Length + messageBytes.Sum(arr => arr.Length) + psnBytes.Length];
    Buffer.BlockCopy(idBytes, 0, output, 0, idBytes.Length);
    Buffer.BlockCopy(typeBytes, 0, output, idBytes.Length, typeBytes.Length);
    Buffer.BlockCopy(messageBytes.SelectMany(arr => arr).ToArray(), 0, output, idBytes.Length + typeBytes.Length, messageBytes.Sum(arr => arr.Length));
    Buffer.BlockCopy(psnBytes, 0, output, idBytes.Length + typeBytes.Length + messageBytes.Sum(arr => arr.Length), psnBytes.Length);

    //Crea y escribe el mensaje en el archivo
    DirectoryInfo mDir = new(messageDir);
    FileInfo[] files = mDir.GetFiles();
    foreach (var file in files) file.Delete();
    File.Create(messageDir + "\\message.txt").Close();
    File.WriteAllBytes(messageDir + "\\message.txt", output);

    //Prepara los datos para la siguiente iteracion
    id++;

    //Crea las llaves
    KeyCreator(keyValue);

    Console.WriteLine("Mensaje enviado. ID del mensaje: " + id);
    Console.WriteLine("Tipo de mensaje: KU. Llaves reestablecidas correctamente.");
    Console.WriteLine("Presione cualquier tecla para continuar...");
    Console.ReadKey();
}



void KeyCreator (int keyValue)
{
    int o = 1;
    ulong P0 = 0, Q0 = 0;
    for (int i = keyValue; i > 0; i--)
    {
        if (o % 2 != 0) //Ejecuta la condicion inicial de llaves
        {
            P0 = Scramble(P, S.Last());
            keys.Add(Generation(P0, Q));
            S.Add(Mutator(S.Last(), Q));
        }
        else //Ejecuta la segunda iteracion de llaves
        {
            Q0 = Scramble(Q, S.Last());
            keys.Add(Generation(Q0, P0));
            S.Add(Mutator(S.Last(), P0));
        }
        o++;
    }
}

List<byte[]> messageConverter (string message)
{
    List<byte[]> messageBytes = new();
    for (int i = 0; i < message.Length; i += 8)
    {
        string chunk = message.Substring(i, Math.Min(8, message.Length - i));
        byte[] chunkBytes = Encoding.UTF8.GetBytes(chunk);
        messageBytes.Add(chunkBytes);
    }
    return messageBytes;
}


//fs (a, b) = a * (2b + 1)
ulong Scramble(ulong a, ulong b)
{
    return a * ((2 * b) + 1);
}

//fg (a, b) = 2a + b
ulong Generation(ulong a, ulong b)
{
    return (2 * a) + b;
}

//fm (a, b) = ab + 2a
ulong Mutator(ulong a, ulong b)
{
    return (a * b) + (2 * a);
}
