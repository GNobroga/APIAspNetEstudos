
using System.Runtime.CompilerServices;

var http = new HttpClient();

var result = http.GetAsync("https://jsonplaceholder.typicode.com/users").Result;



using var stream = new StreamReader(result.Content.ReadAsStream());

int character;

using var fileStream = new FileStream("./file.txt", FileMode.Create, FileAccess.ReadWrite);

using var output = new StreamWriter(fileStream);

var buffer = new char[1024];
int qntBuffer = 0;

while ((qntBuffer = stream.Read(buffer, 0, buffer.Length)) > 0)
{
    output.Write(buffer, 0, qntBuffer);
}