using System.Text;

//Directorio de mensajes
string messageDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\Messages\\";

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
        type = type.Trim(' ');

        switch (type)
        {
            case "FCM":
                FCM(reader);
                break;
        }


        Console.ReadKey();
    }
}


void FCM(byte[] input)
{
    //Obtener el ID de mensaje
    byte[] idBytes = new byte[8];
    Buffer.BlockCopy(input, 0, idBytes, 0, 8);
    ulong id = BitConverter.ToUInt64(idBytes);

    //Obtener el mensaje
    byte[] messageBytes = new byte[input.Length - 16];
    Buffer.BlockCopy(input, 12, messageBytes, 0, input.Length - 16);
    string message = Encoding.UTF8.GetString(messageBytes);


    Console.ReadKey();

}

//void KeyCreator(int keyValue)
//{
//    int o = 1;
//    ulong P0 = 0, Q0 = 0;
//    for (int i = keyValue; i > 0; i--)
//    {
//        if (o % 2 != 0) //Ejecuta la condicion inicial de llaves
//        {
//            P0 = Scramble(P, S.Last());
//            keys.Add(Generation(P0, Q));
//            S.Add(Mutator(S.Last(), Q));
//        }
//        else //Ejecuta la segunda iteracion de llaves
//        {
//            Q0 = Scramble(Q, S.Last());
//            keys.Add(Generation(Q0, P0));
//            S.Add(Mutator(S.Last(), P0));
//        }
//        o++;
//    }
//}

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
