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
    
    