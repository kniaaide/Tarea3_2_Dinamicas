namespace Tarea3_2_Dinamicas
{
    /// <summary>
    /// Nodo de enlace doble: dato + referencia al nodo anterior y al siguiente.
    /// Cuesta un puntero extra por nodo, pero permite recorrer en ambos sentidos
    /// y eliminar un nodo conocido en O(1).
    /// </summary>
    public class NodoDoble<T>
    {
        public T Dato { get; set; }
        public NodoDoble<T> Anterior { get; set; }
        public NodoDoble<T> Siguiente { get; set; }

        public NodoDoble(T dato)
        {
            Dato = dato;
        }
    }
}
