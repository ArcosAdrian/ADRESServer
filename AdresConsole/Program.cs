using AdresLib;
using System;  

namespace AdresConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {

            AdresService service =  new AdresService();

            try
            {
                using (var conn = service.getConnection()) 
                {
                    conn.Open();
                    Console.WriteLine($"Conexion  Correcta");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al abrir la conexion : {ex.Message}");
            }

            try
            {
                var servicios = service.ServiciosListar();
                Console.WriteLine($"ServiciosListar  OK, {servicios.Count} retornados");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar Listar : {ex.Message}");
            }



            Console.WriteLine("FIN!");
        }
    }
}
