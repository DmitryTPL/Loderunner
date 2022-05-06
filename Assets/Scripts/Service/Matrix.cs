using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loderunner.Service
{
    [Serializable]
    public class Matrix<T> : ISerializationCallbackReceiver, IEnumerable<T>
    {
        [SerializeField] private T[] _array;
        [SerializeField] private int _rows;
        [SerializeField] private int _columns;

        private int _previousRows;
        private int _previousColumns;

        public T this[int row, int column]
        {
            get => _array[column + row * _columns];
            set => _array[column + row * _columns] = value;
        }

        public int Length => _array.Length;

        public int Rows => _rows;

        public int Columns => _columns;

        public Matrix(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;

            _array = new T[rows * columns];
        }

        public void OnBeforeSerialize()
        {
            _array ??= new T[_rows * _columns];

            if (_previousColumns == 0)
            {
                _previousColumns = _columns;
            }

            if (_previousRows == 0)
            {
                _previousRows = _rows;
            }

            if (_array.Length == 0 && _columns != 0 && _rows != 0)
            {
                _array = new T[_rows * _columns];
            }

            if (_previousColumns != _columns || _previousRows != _rows)
            {
                var newArray = new T[_rows * _columns];

                if (_array.Length == 0)
                {
                    _array = newArray;
                    _previousColumns = _columns;
                    _previousRows = _rows;
                    return;
                }

                var rows = Math.Min(_rows, _previousRows);
                var columns = Math.Min(_columns, _previousColumns);

                for (var row = 0; row < rows; row++)
                {
                    for (var column = 0; column < columns; column++)
                    {
                        var value = _array[column + row * _previousColumns];
                        newArray[column + row * _columns] = value;
                    }
                }

                _array = newArray;
                _previousColumns = _columns;
                _previousRows = _rows;
            }
        }

        public void OnAfterDeserialize()
        {
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_array).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}