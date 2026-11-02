using System;

namespace Tarea3_2_Dinamicas
{
    /// <summary>
    /// COLA CIRCULAR ESTÁTICA.
    /// El arreglo se trata como un anillo: tras el último índice se vuelve al 0 mediante
    /// aritmética modular  (índice + 1) % Capacidad.  Así se reutilizan SIEMPRE las celdas
    /// liberadas y se elimina el desperdicio de la cola simple.
    ///
    /// Para distinguir "vacía" de "llena" (en ambos casos front y rear quedarían contiguos)
    /// se mantiene un contador de elementos (_count).
    /// Complejidad: todas las operaciones son O(1).
    /// </summary>
    public class ColaCircularEstatica<T>
    {
        private readonly T[] _datos;
        private int _front;   // Índice del próximo elemento a salir
        private int _rear;    // Índice del último elemento insertado (-1 mientras no se ha insertado nada)
        private int _count;

        public int Capacidad => _datos.Length;
        public int IndiceFront => _front;
        public int IndiceRear => _rear;
        public int Count => _count;

        public ColaCircularEstatica(int capacidad)
        {
            if (capacidad <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacidad), "La capacidad debe ser mayor que 0.");
            _datos = new T[capacidad];
            _front = 0;
            _rear = -1;
            _count = 0;
        }

        public bool IsEmpty() => _count == 0;

        public bool IsFull() => _count == _datos.Length;

        public void Enqueue(T elemento)
        {
            if (IsFull())
                throw new InvalidOperationException("Cola circular llena: todas las celdas están ocupadas.");
            _rear = (_rear + 1) % _datos.Length;   // Avance circular
            _datos[_rear] = elemento;
            _count++;
        }

        public T Dequeue()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Subdesbordamiento (underflow): la cola está vacía.");
            T valor = _datos[_front];
            _datos[_front] = default(T);
            _front = (_front + 1) % _datos.Length; // Avance circular
            _count--;
            return valor;
        }

        public T Peek()
        {
            if (IsEmpty())
                throw new InvalidOperationException("La cola está vacía: no hay elemento al frente.");
            return _datos[_front];
        }

        public T[] ObtenerArreglo()
        {
            T[] copia = new T[_datos.Length];
            for (int i = 0; i < _datos.Length; i++)
                copia[i] = _datos[i];
            return copia;
        }
    }
}
