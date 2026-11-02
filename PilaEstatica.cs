using System;

namespace Tarea3_2_Dinamicas
{
    /// <summary>
    /// PILA ESTÁTICA (LIFO: Last In, First Out).
    /// Implementada con un arreglo de tamaño fijo y un único índice "top".
    /// NO usa List&lt;T&gt;, Stack&lt;T&gt; ni ninguna colección de System.Collections.
    ///
    /// Convención: top = -1 indica pila vacía; top = Capacidad - 1 indica pila llena.
    /// Complejidad: Push, Pop, Peek, IsEmpty e IsFull son O(1).
    /// </summary>
    /// <typeparam name="T">Tipo de dato almacenado.</typeparam>
    public class PilaEstatica<T>
    {
        private readonly T[] _datos;   // Almacenamiento: memoria reservada de golpe
        private int _top;              // Índice del último elemento insertado

        /// <summary>Número máximo de elementos (fijo desde la creación).</summary>
        public int Capacidad => _datos.Length;

        /// <summary>Cantidad de elementos almacenados actualmente.</summary>
        public int Count => _top + 1;

        /// <summary>Índice del tope (-1 si está vacía). Se expone para visualizarlo en la interfaz.</summary>
        public int Top => _top;

        public PilaEstatica(int capacidad)
        {
            if (capacidad <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacidad), "La capacidad debe ser mayor que 0.");
            _datos = new T[capacidad];
            _top = -1;
        }

        /// <summary>Devuelve true si la pila no contiene elementos.</summary>
        public bool IsEmpty() => _top == -1;

        /// <summary>Devuelve true si la pila ya usó toda su capacidad.</summary>
        public bool IsFull() => _top == _datos.Length - 1;

        /// <summary>Inserta un elemento en el tope. Lanza excepción si hay desbordamiento (overflow).</summary>
        public void Push(T elemento)
        {
            if (IsFull())
                throw new InvalidOperationException("Desbordamiento (overflow): la pila está llena.");
            _top++;
            _datos[_top] = elemento;
        }

        /// <summary>Elimina y devuelve el elemento del tope. Lanza excepción si está vacía (underflow).</summary>
        public T Pop()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Subdesbordamiento (underflow): la pila está vacía.");
            T valor = _datos[_top];
            _datos[_top] = default(T);   // Limpia la celda (evita dejar referencias "fantasma")
            _top--;
            return valor;
        }

        /// <summary>Devuelve el elemento del tope SIN eliminarlo.</summary>
        public T Peek()
        {
            if (IsEmpty())
                throw new InvalidOperationException("La pila está vacía: no hay elemento en el tope.");
            return _datos[_top];
        }

        /// <summary>Copia del arreglo interno (solo para dibujar la estructura en pantalla).</summary>
        public T[] ObtenerArreglo()
        {
            T[] copia = new T[_datos.Length];
            for (int i = 0; i < _datos.Length; i++)
                copia[i] = _datos[i];
            return copia;
        }
    }
}
