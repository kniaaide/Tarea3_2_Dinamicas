using System;
using System.Collections;
using System.Collections.Generic;

namespace Tarea3_2_Dinamicas
{
    /// <summary>
    /// LISTA DOBLEMENTE ENLAZADA con referencias a cabeza y cola.
    /// Implementada con nodos propios (NodoDoble&lt;T&gt;): no usa List&lt;T&gt; ni LinkedList&lt;T&gt;.
    /// (Implementa IEnumerable&lt;T&gt; solo para poder recorrerla con foreach y mostrarla en pantalla.)
    ///
    /// Complejidades:
    ///   InsertarInicio / InsertarFinal / EliminarInicio / EliminarFinal ... O(1)
    ///   InsertarEn / EliminarEn / Obtener / Buscar / EliminarValor ........ O(n)
    ///   Invertir .......................................................... O(n)
    /// </summary>
    public class ListaDoble<T> : IEnumerable<T>
    {
        private NodoDoble<T> _cabeza;
        private NodoDoble<T> _cola;
        private int _cuenta;

        public int Count => _cuenta;

        public bool IsEmpty() => _cuenta == 0;

        // ------------------------------------------------------------ INSERCIÓN
        public void InsertarInicio(T dato)
        {
            NodoDoble<T> nuevo = new NodoDoble<T>(dato);
            if (_cabeza == null)
            {
                _cabeza = _cola = nuevo;          // Lista vacía: el nodo es cabeza y cola
            }
            else
            {
                nuevo.Siguiente = _cabeza;
                _cabeza.Anterior = nuevo;
                _cabeza = nuevo;
            }
            _cuenta++;
        }

        public void InsertarFinal(T dato)
        {
            NodoDoble<T> nuevo = new NodoDoble<T>(dato);
            if (_cola == null)
            {
                _cabeza = _cola = nuevo;
            }
            else
            {
                nuevo.Anterior = _cola;
                _cola.Siguiente = nuevo;
                _cola = nuevo;
            }
            _cuenta++;
        }

        /// <summary>Inserta de modo que el nuevo nodo quede en la posición "indice" (0..Count).</summary>
        public void InsertarEn(int indice, T dato)
        {
            if (indice < 0 || indice > _cuenta) throw RangoInvalido(indice, _cuenta);
            if (indice == 0) { InsertarInicio(dato); return; }
            if (indice == _cuenta) { InsertarFinal(dato); return; }

            NodoDoble<T> actual = NodoEn(indice);   // Nodo que quedará DESPUÉS del nuevo
            NodoDoble<T> nuevo = new NodoDoble<T>(dato)
            {
                Anterior = actual.Anterior,
                Siguiente = actual
            };
            actual.Anterior.Siguiente = nuevo;
            actual.Anterior = nuevo;
            _cuenta++;
        }

        // ------------------------------------------------------------ ELIMINACIÓN
        public T EliminarInicio()
        {
            if (IsEmpty()) throw new InvalidOperationException("La lista está vacía.");
            NodoDoble<T> nodo = _cabeza;
            T dato = nodo.Dato;
            Desenlazar(nodo);
            return dato;
        }

        public T EliminarFinal()
        {
            if (IsEmpty()) throw new InvalidOperationException("La lista está vacía.");
            NodoDoble<T> nodo = _cola;
            T dato = nodo.Dato;
            Desenlazar(nodo);
            return dato;
        }

        public T EliminarEn(int indice)
        {
            if (indice < 0 || indice >= _cuenta) throw RangoInvalido(indice, _cuenta - 1);
            NodoDoble<T> nodo = NodoEn(indice);
            T dato = nodo.Dato;
            Desenlazar(nodo);
            return dato;
        }

        /// <summary>Elimina la primera aparición del valor. Devuelve false si no existe.</summary>
        public bool EliminarValor(T dato)
        {
            EqualityComparer<T> comparador = EqualityComparer<T>.Default;
            for (NodoDoble<T> a = _cabeza; a != null; a = a.Siguiente)
            {
                if (comparador.Equals(a.Dato, dato))
                {
                    Desenlazar(a);
                    return true;
                }
            }
            return false;
        }

        // ------------------------------------------------------------ CONSULTA
        public int Buscar(T dato)
        {
            EqualityComparer<T> comparador = EqualityComparer<T>.Default;
            int i = 0;
            for (NodoDoble<T> a = _cabeza; a != null; a = a.Siguiente, i++)
                if (comparador.Equals(a.Dato, dato)) return i;
            return -1;
        }

        public T Obtener(int indice)
        {
            if (indice < 0 || indice >= _cuenta) throw RangoInvalido(indice, _cuenta - 1);
            return NodoEn(indice).Dato;
        }

        // ------------------------------------------------------------ OTRAS
        /// <summary>Invierte la lista intercambiando Anterior/Siguiente de cada nodo (in situ, O(n)).</summary>
        public void Invertir()
        {
            NodoDoble<T> actual = _cabeza;
            while (actual != null)
            {
                NodoDoble<T> siguienteOriginal = actual.Siguiente;
                actual.Siguiente = actual.Anterior;
                actual.Anterior = siguienteOriginal;
                actual = siguienteOriginal;
            }
            NodoDoble<T> temporal = _cabeza;
            _cabeza = _cola;
            _cola = temporal;
        }

        public void Limpiar()
        {
            _cabeza = _cola = null;
            _cuenta = 0;
        }

        // ------------------------------------------------------------ INTERNOS
        /// <summary>Recorre desde el extremo más cercano al índice (optimización de la lista doble).</summary>
        private NodoDoble<T> NodoEn(int indice)
        {
            NodoDoble<T> actual;
            if (indice < _cuenta / 2)
            {
                actual = _cabeza;
                for (int i = 0; i < indice; i++) actual = actual.Siguiente;
            }
            else
            {
                actual = _cola;
                for (int i = _cuenta - 1; i > indice; i--) actual = actual.Anterior;
            }
            return actual;
        }

        /// <summary>Quita un nodo conocido de la lista en O(1) reconectando a sus vecinos.</summary>
        private void Desenlazar(NodoDoble<T> nodo)
        {
            if (nodo.Anterior != null) nodo.Anterior.Siguiente = nodo.Siguiente; else _cabeza = nodo.Siguiente;
            if (nodo.Siguiente != null) nodo.Siguiente.Anterior = nodo.Anterior; else _cola = nodo.Anterior;
            nodo.Anterior = null;
            nodo.Siguiente = null;
            _cuenta--;
        }

        private static ArgumentOutOfRangeException RangoInvalido(int indice, int maximo) =>
            new ArgumentOutOfRangeException((string)null, $"La posición {indice} está fuera de rango (válido: 0 a {Math.Max(maximo, 0)}).");

        public IEnumerable<T> Reversa()
        {
            for (NodoDoble<T> a = _cola; a != null; a = a.Anterior) yield return a.Dato;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (NodoDoble<T> a = _cabeza; a != null; a = a.Siguiente) yield return a.Dato;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
