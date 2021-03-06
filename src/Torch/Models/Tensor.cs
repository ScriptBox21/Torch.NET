﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Python.Runtime;
using Numpy.Models;

namespace Torch
{
    public partial class Tensor : PythonObject
    {
        public Tensor(PyObject pyobj) : base(pyobj)
        {
        }

        public Tensor(Tensor t) : base((PyObject)t.PyObject)
        {
        }

        /// <summary>
        /// Returns a copy of the tensor data
        /// </summary>
        public T[] GetData<T>()
        {
            var storage = PyObject.storage();
            int size = storage.size();
            if (size==0)
                return new T[0];
            long ptr = storage.data_ptr();
            object array = null;
            if (typeof(T) == typeof(byte)) array = new byte[size];
            else if (typeof(T) == typeof(short)) array = new short[size];
            else if (typeof(T) == typeof(int)) array = new int[size];
            else if (typeof(T) == typeof(long)) array = new long[size];
            else if (typeof(T) == typeof(float)) array = new float[size];
            else if (typeof(T) == typeof(double)) array = new double[size];
            switch (array)
            {
                case byte[] a: Marshal.Copy(new IntPtr(ptr), a, 0, a.Length); break;
                case short[] a: Marshal.Copy(new IntPtr(ptr), a, 0, a.Length); break;
                case int[] a: Marshal.Copy(new IntPtr(ptr), a, 0, a.Length); break;
                case long[] a: Marshal.Copy(new IntPtr(ptr), a, 0, a.Length); break;
                case float[] a: Marshal.Copy(new IntPtr(ptr), a, 0, a.Length); break;
                case double[] a: Marshal.Copy(new IntPtr(ptr), a, 0, a.Length); break;
            }
            return (T[])array;
        }

        /// <summary>
        /// Element type of the tensor
        /// </summary>
        public Dtype dtype => new Dtype(self.GetAttr("dtype"));

        public Tensor<T> AsTensor<T>() => new Tensor<T>(self);

        /// <summary>
        /// Is True if gradients need to be computed for this Tensor, False otherwise.
        /// 
        /// Note
        /// The fact that gradients need to be computed for a Tensor do not mean that the grad
        /// attribute will be populated, see is_leaf for more details.
        /// </summary>
        public bool requires_grad => self.GetAttr("requires_grad").As<bool>();

        public Tensor this[params int[] index]
        {
            get { return new Tensor(self.GetItem(ToTuple(index))); }
            set { self.SetItem(ToTuple(index), value.PyObject); }
        }

        public Tensor this[Tensor selection_indices]
        {
            get { return new Tensor(self.GetItem(selection_indices.PyObject)); }
            set { self.SetItem(selection_indices.PyObject, value.PyObject); }
        }

        private T as_scalar<T>() => self.InvokeMethod("item").As<T>();

        /// <summary>
        /// Returns the value of this tensor as a standard Python number. This only works
        /// for tensors with one element. For other cases, see tolist().
        /// 
        /// This operation is not differentiable.
        /// </summary>
        public T item<T>() => self.InvokeMethod("item").As<T>();

        public Shape Shape => size();

        ///// <summary>
        ///// Returns True if the data type of tensor is a floating point data type i.e.,
        ///// one of torch.float64, torch.float32 and torch.float16.
        ///// </summary>
        //public bool is_floating_point
        //    => PyTorch.Instance.is_floating_point(this);

        /// <summary>
        /// Is True if the Tensor is stored on the GPU, False otherwise.
        /// </summary>
        public bool is_cuda
            => self.GetAttr("is_cuda").As<bool>();

        /// <summary>
        /// This attribute is None by default and becomes a Tensor the first time a call to
        /// backward() computes gradients for self. The attribute will then contain the
        /// gradients computed and future calls to backward() will accumulate (add) gradients
        /// into it.
        /// </summary>
        public Tensor grad
        {
            get
            {
                var t = self.GetAttr("grad");
                if (t.IsNone())
                    return null;
                return new Tensor(t);
            }
        }

        /// <summary>
        /// Return the device of this tensor
        /// </summary>
        public Device device
            => new Device(self.GetAttr("device"));

        /// <summary>
        /// Transpose (see t())
        /// </summary>
        public Tensor T => t();

        /// <summary>
        /// Clamp all elements into the range [ min, max ] and return
        /// a resulting tensor:
        /// 
        /// \[y_i = \begin{cases}
        ///     \text{min} & \text{if } x_i < \text{min} \\
        ///     x_i & \text{if } \text{min} \leq x_i \leq \text{max} \\
        ///     \text{max} & \text{if } x_i > \text{max}
        /// \end{cases}
        /// 
        /// \]
        /// 
        /// <param name="min">
        /// lower-bound of the range to be clamped to
        /// </param>
        /// <param name="max">
        /// upper-bound of the range to be clamped to
        /// </param>
        /// <param name="out">
        /// the output tensor
        /// </param>
        public Tensor clamp(double? min = null, double? max = null, Tensor @out = null)
        {
            //auto-generated code, do not change
            var __self__ = self;
            var kwargs = new PyDict();
            if (min != null) kwargs["min"] = ToPython(min);
            if (max != null) kwargs["max"] = ToPython(max);
            if (@out != null) kwargs["out"] = ToPython(@out);
            dynamic py = __self__.InvokeMethod("clamp", new PyTuple(), kwargs);
            return ToCsharp<Tensor>(py);
        }

    }

    public partial class Tensor<T> : Tensor
    {
        public Tensor(Tensor t) : base(t)
        {
        }

        public Tensor(PyObject pyobject) : base(pyobject)
        {
        }

        public Tensor(T[] data) : base(torch.tensor(data))
        {
        }

        public Tensor(T[,] data) : base(torch.tensor(data))
        {
        }

        public Tensor(T[,,] data) : base(torch.tensor(data))
        {
        }

        /// <summary>
        /// Returns a copy of the tensor data
        /// </summary>
        public T[] GetData() => base.GetData<T>();

        public new T this[int index]
        {
            get { return self.GetItem(index).As<T>(); }
            set { self.SetItem(index, ConverterExtension.ToPython(value)); }
        }

        public new Tensor<T> t() => new Tensor<T>(self.InvokeMethod("t"));

   
    }
}
