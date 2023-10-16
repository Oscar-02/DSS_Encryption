using System.Net.Security;
using System.Numerics;
using System.Text;

//Variables a utilizar
ulong P = 154564121, Q = 541218581, S = 454178946, id = 000001;
int N;


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
i = 0;

if (opt == '1') N = 10;
else
{
    bool valid = false;
    do
    {
        try
        {
            Console.WriteLine("\nPor favor, indique la cantidad de nodos a utilizar:");
            N = int.Parse(Console.ReadLine());
            valid = true;
        }
        catch (Exception)
        {
            Console.WriteLine("ERROR: Ha ingresado un valor invalido");
            i++;
            valid = false;
        }
    }
    while(!valid);
}


string workingDirectory = Environment.CurrentDirectory;
string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
if (!File.Exists(projectDirectory + "\\FCM.txt"))
{
    Console.WriteLine("No se ha realizado un contacto confirmado entre el emisor y el receptor");
    Console.WriteLine("Procediendo a enlazar la conexion con el receptor...");

}
else 
{
    
}
Console.ReadKey();



void Encrypt(int nodes, string type, ulong id)
{


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



