using System;

namespace HotelReservaApp
{
    public static class ValidadorReservas
    {
        public static bool ValidarFechaReserva(DateTime fecha, out string mensaje)
        {
            if (fecha.Date < DateTime.Today) { mensaje = "No se pueden hacer reservas para fechas pasadas"; return false; }
            if ((fecha - DateTime.Today).TotalDays > 365) { mensaje = "No se pueden hacer reservas con más de un año de anticipación"; return false; }
            mensaje = "Fecha válida"; return true;
        }

        public static bool ValidarDuracionEstancia(int noches, out string mensaje)
        {
            if (noches < 1) { mensaje = "La estancia debe ser de al menos 1 noche"; return false; }
            if (noches > 30) { mensaje = "La estancia no puede exceder 30 noches"; return false; }
            mensaje = "Duración válida"; return true;
        }

        public static bool ValidarNombre(string nombre, out string mensaje)
        {
            if (string.IsNullOrWhiteSpace(nombre) || nombre.Length < 3) { mensaje = "Nombre inválido (>=3)"; return false; }
            mensaje = "Nombre válido"; return true;
        }

        public static bool ValidarTelefono(string telefono, out string mensaje)
        {
            if (string.IsNullOrWhiteSpace(telefono) || telefono.Length < 8) { mensaje = "Teléfono inválido (>=8)"; return false; }
            mensaje = "Teléfono válido"; return true;
        }

        public static bool ValidarEmail(string email, out string mensaje)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@")) { mensaje = "Email inválido"; return false; }
            mensaje = "Email válido"; return true;
        }

        public static bool ValidarReservaInfo(ReservaInfo info, out string errores)
        {
            var sb = new System.Text.StringBuilder();
            if (!ValidarNombre(info.Nombre, out var m)) sb.AppendLine(m);
            if (!ValidarTelefono(info.Telefono, out m)) sb.AppendLine(m);
            if (!ValidarEmail(info.Email, out m)) sb.AppendLine(m);
            if (!ValidarFechaReserva(info.Start, out m)) sb.AppendLine(m);
            if (!ValidarDuracionEstancia(info.Nights, out m)) sb.AppendLine(m);
            errores = sb.ToString();
            return string.IsNullOrEmpty(errores);
        }
    }
}
