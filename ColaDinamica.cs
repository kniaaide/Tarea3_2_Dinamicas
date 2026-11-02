using System;
using System.Collections;
using System.Collections.Generic;

namespace Tarea3_2_Dinamicas
{
    /// <summary>
    /// COLA DINÁMICA (FIFO) basada en nodos enlazados con punteros a frente y final.
    /// Enqueue agrega al final y Dequeue quita del frente, ambos en O(1).
    /// Al ser dinámica no sufre el desperdicio de la cola simple estática.
    /// </summary>
    public class ColaDinamica<T> : IEnumerable<T>
    {
        private Nodo<T> _frente;
        private Nodo<T> _final;
        private int _cuenta;

        public int Count => _cuenta;

        public bool IsEmpty() => _frente == null;

        public void Enqueue(T elemento)
        {
            Nodo<T> nuevo = new Nodo<T>(elemento);
            if (_final == null)
            {
                _frente = _final = nuevo;     // Primera inserción
            }
            else
            {
                _final.Siguiente = nuevo;
                _final = nuevo;
            }
            _cuenta++;
        }

        public T Dequeue()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Subdesbordamiento (underflow): la cola está vacía.");
            T dato = _frente.Dato;
            _frente = _frente.Siguiente;
            if (_frente == null) _final = null;   // Se vació: también se limpia el puntero final
            _cuenta--;
            return dato;
        }

        public T Peek()
        {
            if (IsEmpty())
                throw new InvalidOperationException("La cola está vacía: no hay elemento al frente.");
            return _frente.Dato;
        }

        public void Clear()
        {
            _frente = _final = null;
            _cuenta = 0;
        }

        /// <summary>Recorre del frente hacia el final.</summary>
        public IEnumerator<T> GetEnumerator()
        {
            for (Nodo<T> a = _frente; a != null; a = a.Siguiente) yield return a.Dato;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
