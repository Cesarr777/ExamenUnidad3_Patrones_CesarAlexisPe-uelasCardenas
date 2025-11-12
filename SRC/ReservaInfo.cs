using System;

namespace HotelReservaApp
{
    public class ReservaInfo
    {
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public double DiscountPercent { get; set; } = 0.0;
        public string DiscountReason { get; set; } = string.Empty;

        public override string ToString()
        {
            var d = DiscountPercent > 0 ? $" | Descuento: {DiscountPercent}% ({DiscountReason})" : string.Empty;
            return $"{Nombre} | {Telefono} | {Email} | Inicio: {Start:yyyy-MM-dd} | Noches: {Nights}{d}";
        }

        public bool EsValida(out string errores)
        {
            return ValidadorReservas.ValidarReservaInfo(this, out errores);
        }
    }
}
