using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using MongoDB.Bson;//para poder usar BsonDocument se instalo el paquete NuGet MongoDB.Bson con el comando dotnet add package MongoDB.Bson
using MongoDB.Driver;//para poder usar MongoClient se instalo el paquete NuGet MongoDB.Driver con el comando dotnet add package MongoDB.Driver

namespace LeerCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            string rutaArchivo = @"C:\Users\Emanuel.Casulo\Desktop\Extraer datos csv\Accounting-prueba.csv";

            bool primeraFila = true;

            using (TextFieldParser parser = new TextFieldParser(rutaArchivo))//sirve para leer el archivo
            {
                parser.Delimiters = new string[] { ";" };//para que tome el punto y coma como separador
                parser.HasFieldsEnclosedInQuotes = true;//para que tome los campos que estan entre comillas

                string[] campos;
                var client = new MongoClient("mongodb://localhost:27017"); // reemplaza con tu cadena de conexión
                var database = client.GetDatabase("tomaDeContadores"); // reemplaza con el nombre de tu base de datos
                var collection = database.GetCollection<BsonDocument>("clientes"); // reemplaza con el nombre de tu colección

                while (!parser.EndOfData)//mientras no llegue al final del archivo
                {
                    campos = parser.ReadFields();//sirve para leer los campos de la fila

                    if (primeraFila)
                    {
                        primeraFila = false; // saltar la primera fila
                        continue;
                    }

                    // Acceder a cada columna
                    string ip = campos[0];
                    string serial = campos[1];
                    string modelo = campos[2];
                    string printerBW = campos[3];
                    string copierBW = campos[4];
                    string contadorTotalBW = campos[5];
                    string copierColor = campos[6];
                    string printerColor = campos[7];
                    string contadorTotalColor = campos[8];

                    // Crear un documento BSON
                    var document = new BsonDocument
                    {
                        { "ip", ip },
                        { "serial", serial },
                        { "modelo", modelo },
                        { "printerBW", printerBW },
                        { "copierBW", copierBW },
                        { "contadorTotalBW", contadorTotalBW },
                        { "copierColor", copierColor },
                        { "printerColor", printerColor },
                        { "contadorTotalColor", contadorTotalColor }
                    };

                    // Insertar el documento en la colección
                    collection.InsertOne(document);
                }
            }
        }
    }
}