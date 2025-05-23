using AdresLib.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdresLib
{
    internal static class AdresServiceUtil
    {
        public static List<string> AdquisicionObtenerCambios(Adquisicion anterior, Adquisicion nuevo)
        {
            var cambios = new List<string>();
            var propiedades = typeof(Adquisicion).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in propiedades)
            {
                var valorAnterior = string.Empty;
                var valorNuevo = string.Empty;
                if (anterior != null)
                {
                    valorAnterior = Convert.ToString(prop.GetValue(anterior));
                }
                if (nuevo != null)
                {
                    valorNuevo = Convert.ToString(prop.GetValue(nuevo));
                }
               
                if ((valorAnterior?.ToString() ?? "") != (valorNuevo?.ToString() ?? ""))
                {
                    cambios.Add($"{prop.Name}: '{valorAnterior}' → '{valorNuevo}'");
                }
            }

            return cambios;
        }

        /// <summary>
        /// Valida que una Adquisicion este correctamente diligenciada
        /// </summary>
        /// <param name="adquisicion"></param>
        public static void AdquisicionValidar(Adquisicion adquisicion, Accion accion)
        {
            if (accion == Accion.Eliminar || accion == Accion.Modificar)
            {
                if (!adquisicion.Id.HasValue)
                {
                    new ArgumentException("El id de la adquisicion no puede ser nulo");
                }
            }

            if (adquisicion.Cantidad < 0)
            {
                throw new ArgumentException("La cantidad no puede ser negativa");
            }
            // Fecha
            if (string.IsNullOrEmpty(adquisicion.FechaAdquisicion))
            {
                throw new ArgumentException("La Fecha de Adquisicion no puede estar vacia");
            }
            DateTime fecha;
            bool esFecha = DateTime.TryParse(adquisicion.FechaAdquisicion, out fecha);
            if (!esFecha)
            {
                throw new ArgumentException($"El parametroFecha de Adquisicion {adquisicion.FechaAdquisicion} no puede convertirse a una Fecha Valida");
            }
        }
        
        /// <summary>
        /// Recupera el usuario que ejeuta la accion
        /// </summary>
        /// <returns>Usuario logueado</returns>
        public static string ObtenerUsuario()
        {
            // En un entorno real, el dato se podria obtener desde el token de un JWT.
            // En este caso, se retorna un usuario quemado.
            return "SYSTEM";
        }
    }
}
