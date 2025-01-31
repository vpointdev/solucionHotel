﻿using System;

namespace Entidades.SQLServer
{
    public class Habitacion
    {
        public int HabitacionId { get; set; }
        public string NumeroHabitacion { get; set; }
        public int TipoHabitacionId { get; set; }
        public int Piso { get; set; }
        public string Estado { get; set; } = "Disponible";
        public string Observaciones { get; set; }
        public bool Activo { get; set; } = true;
    }
}