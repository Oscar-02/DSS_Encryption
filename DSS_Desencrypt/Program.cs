using System.Text;

//Variables a utilizar
ulong P, Q, id = 0;
int N = 10;
List<ulong> S = new();
string? message = "";
string PSN = "    ";
List<ulong> keys = new();

//Directorio de mensajes
var location = AppDomain.CurrentDomain.BaseDirectory;
string messageDir = Directory.GetParent(location).Parent.Parent.Parent.Parent.FullName + "\\Messages\\";

while (true)
{
    Console.Clear();
    Console.WriteLine("Desencriptador de mensajes");
    Console.WriteLine("Presione cualquier tecla para verificar si hay un nuevo mensaje...");
    Console.ReadKey();

    DirectoryInfo dir = new DirectoryInfo(messageDir);
    FileInfo[] files = dir.GetFiles("*.txt");

    if (files.Length == 0)
    {
        Console.WriteLine("\nNo hay mensajes nuevos. Presione cualquier tecla para continuar...");
        Console.ReadKey();
    }
    else
    {
        byte[] reader = File.ReadAllBytes(files[0].FullName);

        byte[] typeByte = new byte[4];
        Buffer.BlockCopy(reader, 8, typeByte, 0, 4);
        string type = Encoding.UTF8.GetString(typeByte);

        byte[] idBytes = new byte[8];
        Buffer.BlockCopy(reader, 0, idBytes, 0, 8);
        ulong idTemp = BitConverter.ToUInt64(idBytes);

        if (idTemp == id)
        {
            Console.WriteLine("\nNo hay mensajes nuevos. Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        else
        {
            type = type.Trim(' ');

            switch (type)
            {
                case "FCM":
                    FCM(reader);
                    break;
                case "KU":
                    KU(reader);
                    break;
            }
        }

    }
}


void FCM(byte[] input)
{
    //Obtener el ID de mensaje
    byte[] idBytes = new byte[8];
    Buffer.BlockCopy(input, 0, idBytes, 0, 8);
    id = BitConverter.ToUInt64(idBytes);

    //Obtener el mensaje
    byte[] messageBytes = new byte[input.Length - 16];
    Buffer.BlockCopy(input, 12, messageBytes, 0, input.Length - 16);
    string message = Encoding.UTF8.GetString(messageBytes);

    //FCM => Obtener los valores para las llaves
    P = ulong.Parse(message.Split('.')[0]);
    Q = ulong.Parse(message.Split('.')[1]);
    S.Add(ulong.Parse(message.Split('.')[2]));
    int keyValue = int.Parse(message.Split('.')[3]);
    KeyCreator(keyValue);

    //Imprime los datos del mensaje recibido
    Console.WriteLine("Mensaje recibido. ID del mensaje: " + id);
    Console.WriteLine("Tipo de mensaje: FCM. Contacto establecido correctamente.");
    Console.WriteLine("Presione cualquier tecla para continuar...");
    Console.ReadKey();

    DirectoryInfo mDir = new(messageDir);
    FileInfo[] files = mDir.GetFiles();
    foreach (var file in files) file.Delete();
}

void KU(byte[] input)
{
    //Obtener el ID de mensaje
    byte[] idBytes = new byte[8];
    Buffer.BlockCopy(input, 0, idBytes, 0, 8);
    id = BitConverter.ToUInt64(idBytes);

    //Obtener el mensaje
    byte[] messageBytes = new byte[input.Length - 16];
    Buffer.BlockCopy(input, 12, messageBytes, 0, input.Length - 16);
    string message = Encoding.UTF8.GetString(messageBytes);

    //FCM => Obtener los valores para las llaves
    P = ulong.Parse(message.Split('.')[0]);
    Q = ulong.Parse(message.Split('.')[1]);
    S.Add(ulong.Parse(message.Split('.')[2]));
    int keyValue = int.Parse(message.Split('.')[3]);
    KeyCreator(keyValue);

    //Imprime los datos del mensaje recibido
    Console.WriteLine("Mensaje recibido. ID del mensaje: " + id);
    Console.WriteLine("Tipo de mensaje: KU. Llaves reestablecidas correctamente.");
    Console.WriteLine("Presione cualquier tecla para continuar...");
    Console.ReadKey();

    DirectoryInfo mDir = new(messageDir);
    FileInfo[] files = mDir.GetFiles();
    foreach (var file in files) file.Delete();
}

void KeyCreator(int keyValue)
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
