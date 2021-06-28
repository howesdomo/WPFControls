using System;
using System.Collections;
using System.Collections.Generic;

using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace System.Windows.Controls
{
	// [Xaml.TypeConversion(typeof(RowDefinitionCollection))]	
	[TypeConverterAttribute(typeof(RowDefinitionCollectionTypeConverter))]
	public class RowDefinitionCollectionTypeConverter : TypeConverter
	{
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
    //        if (value != null)
    //        {
    //            string valueStr = value.ToString();

    //            var lengths = valueStr.Split(',');				
				//var coldefs = new RowDefinitionCollection();
    //            var converter = new GridLengthTypeConverter();
    //            foreach (var length in lengths)
    //                coldefs.Add(new RowDefinition { Height = (GridLength)converter.ConvertFromInvariantString(length) });
    //            return coldefs;
    //        }

            throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", value, typeof(RowDefinitionCollection)));
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (!(value is RowDefinitionCollection rdc))
                throw new NotSupportedException();
            var converter = new GridLengthTypeConverter();
            return string.Join(", ", rdc.Select(rd => converter.ConvertToInvariantString(rd.Height)));
        }
    }

    // [Xaml.TypeConversion(typeof(GridLength))]
    [TypeConverterAttribute(typeof(GridLengthTypeConverter))]
    public class GridLengthTypeConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object argsValue)
        {
            if (argsValue == null)
                return null;

            string value = argsValue.ToString();

            value = value.Trim();
            if (string.Compare(value, "auto", StringComparison.OrdinalIgnoreCase) == 0)
                return GridLength.Auto;
            if (string.Compare(value, "*", StringComparison.OrdinalIgnoreCase) == 0)
                return new GridLength(1, GridUnitType.Star);
            if (value.EndsWith("*", StringComparison.Ordinal) && double.TryParse(value.Substring(0, value.Length - 1), NumberStyles.Number, CultureInfo.InvariantCulture, out var length))
                return new GridLength(length, GridUnitType.Star);
            if (double.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out length))
                return new GridLength(length);

            throw new FormatException();
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (!(value is GridLength length))
                throw new NotSupportedException();
            if (length.IsAuto)
                return "auto";
            if (length.IsStar)
                return $"{length.Value.ToString(CultureInfo.InvariantCulture)}*";
            return $"{length.Value.ToString(CultureInfo.InvariantCulture)}";
        }
    }

	public class DefinitionCollection<T> : IList<T>, ICollection<T> where T : IDefinition
	{
		readonly List<T> _internalList = new List<T>();

		internal DefinitionCollection()
		{
		}

		public void Add(T item)
		{
			_internalList.Add(item);
			item.SizeChanged += OnItemSizeChanged;
			OnItemSizeChanged(this, EventArgs.Empty);
		}

		public void Clear()
		{
			foreach (T item in _internalList)
				item.SizeChanged -= OnItemSizeChanged;
			_internalList.Clear();
			OnItemSizeChanged(this, EventArgs.Empty);
		}

		public bool Contains(T item)
		{
			return _internalList.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			_internalList.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _internalList.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(T item)
		{
			item.SizeChanged -= OnItemSizeChanged;
			bool success = _internalList.Remove(item);
			if (success)
				OnItemSizeChanged(this, EventArgs.Empty);
			return success;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _internalList.GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _internalList.GetEnumerator();
		}

		public int IndexOf(T item)
		{
			return _internalList.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			_internalList.Insert(index, item);
			item.SizeChanged += OnItemSizeChanged;
			OnItemSizeChanged(this, EventArgs.Empty);
		}

		public T this[int index]
		{
			get { return _internalList[index]; }
			set
			{
				if (index < _internalList.Count && index >= 0 && _internalList[index] != null)
					_internalList[index].SizeChanged -= OnItemSizeChanged;

				_internalList[index] = value;
				value.SizeChanged += OnItemSizeChanged;
				OnItemSizeChanged(this, EventArgs.Empty);
			}
		}

		public void RemoveAt(int index)
		{
			T item = _internalList[index];
			_internalList.RemoveAt(index);
			item.SizeChanged -= OnItemSizeChanged;
			OnItemSizeChanged(this, EventArgs.Empty);
		}

		public event EventHandler ItemSizeChanged;

		void OnItemSizeChanged(object sender, EventArgs e)
		{
			EventHandler eh = ItemSizeChanged;
			if (eh != null)
				eh(this, EventArgs.Empty);
		}
	}

	//public sealed class RowDefinitionCollection : DefinitionCollection<RowDefinition>
 //   {
 //   }

    public interface IDefinition
    {
        event EventHandler SizeChanged;
    }
}
