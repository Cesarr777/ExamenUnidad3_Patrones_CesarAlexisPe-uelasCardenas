using System;

namespace HotelReservaApp
{
    public class DesayunoDecorator : ProductoDecorator
    {
        private readonly double _costo;
        public DesayunoDecorator(HabitacionBase inner, double costo = 250.0) : base(inner) { _costo = costo; }
        public override double Precio => Math.Round(_inner.Precio + _costo, 2);
        public override string Descripcion => _inner.Descripcion + $" - Desayuno ${_costo:F2} incluido";
    }
}
