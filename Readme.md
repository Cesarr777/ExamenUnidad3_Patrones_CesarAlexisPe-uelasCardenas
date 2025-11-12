# Sistema de Reservas con Validaciones Extendidas  
**Autor:** César Alexis Peñuelas Cárdenas  
**Matrícula:** 22210335  
**Instituto:** Tecnológico de Tijuana  
**Materia:** Patrones de Diseño  
**Horario:** 2–3 PM  

#  Sistema de Reservas de Hotel — Patrones Decorator y Composite

## Descripción General

Este proyecto implemento un **sistema de reservas de habitaciones de hotel** desarrollado en **C#**, aplicando los patrones de diseño **Decorator** y **Composite**.  
Su objetivo es demostrar cómo estructurar un sistema **flexible, extensible y mantenible**, permitiendo agregar dinámicamente funcionalidades (como descuentos o desayuno incluido) y manejar la disponibilidad de habitaciones mediante un calendario compuesto.

---

##  Estructura del Proyecto

El sistema está compuesto por los siguientes módulos principales:

- **Habitaciones (`HabitacionBase` y derivadas)**  
  Representan los distintos tipos de habitaciones del hotel (Deluxe, Premium, Presidencial, etc.).

- **Decoradores (`ProductoDecorator`, `DescuentoDecorator`, `DesayunoDecorator`)**  
  Permiten agregar dinámicamente características adicionales a las habitaciones sin modificar su estructura original.

- **Calendario y Días (`Calendario`, `Dia`)**  
  Implementan el patrón Composite para gestionar las reservas de forma estructurada por días.

- **Reserva (`ReservaInfo` y `ValidadorReservas`)**  
  Manejan los datos de la reserva, validaciones y control de disponibilidad.

---

## Patrón Decorator

###  Dónde se aplica
El **patrón Decorator** se aplica sobre las clases de habitación.  
Permite **añadir funcionalidades adicionales** (como descuentos o desayuno incluido) **sin modificar las clases base**.

### Cómo funciona
- `HabitacionBase` define la estructura y propiedades comunes.
- `ProductoDecorator` actúa como decorador abstracto que envuelve una habitación.
- `DescuentoDecorator` y `DesayunoDecorator` modifican el **precio** y la **descripción** de la habitación decorada.

#### Ejemplo de uso:

```csharp
HabitacionBase habitacion = new HabitacionDeluxe(1);
habitacion = new DescuentoDecorator(habitacion, 0.15);   // Aplica 15% de descuento
habitacion = new DesayunoDecorator(habitacion, 200);     // Agrega desayuno incluido

Console.WriteLine(habitacion.Nombre);
Console.WriteLine($"Precio total: {habitacion.Precio}");
