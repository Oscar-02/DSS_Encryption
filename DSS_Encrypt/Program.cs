using System.Net.Security;
using System.Numerics;
using System.Text;

Console.WriteLine("Bienvenido al cifrador de mensajes.\n");

string? message = "";
char opt;
int i = 0;

do
{
    if (string.IsNullOrWhiteSpace(message) && i > 0) Console.WriteLine("El mensaje no puede estar vacío.\n");
    Console.WriteLine("Para Iniciar, por favor indique el mensaje a encriptar:");
    message = Console.ReadLine();
    i++;
}
while (string.IsNullOrWhiteSpace(message));
i = 0;

string workingDirectory = Environment.CurrentDirectory;
string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
if (!File.Exists(projectDirectory + "\\FCM.txt"))
{
    Console.WriteLine("No se ha realizado un contacto entre el emisor y el receptor");
}

do
{
    if (i > 0) Console.WriteLine("ERROR: Ha seleccionado una opcion invalida");
    Console.WriteLine("\n¿Desea cifrar el mensaje con:");
    Console.WriteLine("1. la cantidad de nodos predeterminados [10]?");
    Console.WriteLine("2. una cantidad de nodos personalizado?");
    opt = Console.ReadKey().KeyChar;
    i++;
}
while (opt != '1' && opt != '2');

Encrypt(2);



void Encrypt(int nodes)
{
    //Define las cargas a utilizar
    ulong P = 154564121, Q = 541218581, S = 454178946;
    int N = nodes;

    //Convierte el mensaje a un arreglo de bytes dividido por chunks y cada chunk lo codifica en un numero ulong
    List<ulong> messageBytes = new();
    for (int i = 0; i < message.Length; i += 8)
    {
        string chunk = message.Substring(i, Math.Min(8, message.Length - i));
        if (chunk.Length < 8)
        {
            int length = chunk.Length;
            for (int j = 0; j < 8 - length; j++)
            {
                chunk += " ";
            }
        }
        byte[] chunkBytes = Encoding.UTF8.GetBytes(chunk);
        ulong chunkNumber = BitConverter.ToUInt64(chunkBytes);
        messageBytes.Add(chunkNumber);
    }

    //Genera un arreglo de llaves aleatorias basada usando dos numeros primos y una semilla
    
    //List<ulong> keys = new();
    //ulong P0 = P, Q0 = Q, seed0 = seed, o = 1;

    //for (int i = nodes; i > 0; i--)
    //{
    //    //Si 'o' es impar, se ejecuta esta serie de funciones
    //    if (o % 2 != 0)
    //    {
    //        P0 = Scramble(P, Q);
    //        ulong key = KeyGen(P0, Q);
    //        byte[] keyBytes = BitConverter.GetBytes(key);
    //        ulong keyNumber = BitConverter.ToUInt64(keyBytes);
    //        keys.Add(keyNumber);
    //        seed0 = Scramble(seed, Q);
    //        o++;
    //    }
    //    //Si 'o' es par, se ejecuta esta serie de funciones
    //    else
    //    {
    //        Q0 = Scramble(Q, seed0);
    //        ulong key = KeyGen(Q0, P0);
    //        byte[] keyBytes = BitConverter.GetBytes(key);
    //        ulong keyNumber = BitConverter.ToUInt64(keyBytes);
    //        keys.Add(keyNumber);
    //        seed = Scramble(seed, Q);
    //        o++;
    //    }

    //}

    //Genera el PSN para que el receptor pueda desencriptar el mensaje
    
    
}



//fs (a, b) = a * (2b + 1)
ulong Scramble(ulong a, ulong b)
{
    return a * ((2 * b) + 1);
}

//fg (a, b) = 2a + b
ulong KeyGen(ulong a, ulong b)
{
    return (2 * a) + b;
}



