using NUnit.Framework;
using QMat_Calculator.Circuits;
using QMat_Calculator.Circuits.Gates;
using QMat_Calculator.Matrices;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace QMat_Calculator.UnitTest
{
    public class Tests
    {
        /// <summary>
        /// Create a testCircuit to perform operations on.
        /// </summary>
        /// <returns></returns>
        public List<Qubit> createCircuit()
        {
            List<Qubit> qubits = new List<Qubit>();     //  H       X       H
                                                        //  H       Y       H
                                                        // Create the qubits
            Qubit q1 = new Qubit();
            Qubit q2 = new Qubit();

            // Populate the Qubits
            q1.addGate(new Hadamard());
            q1.addGate(new Pauli(Pauli.PauliType.X));
            q1.addGate(new Hadamard());

            q2.addGate(new Hadamard());
            q2.addGate(new Pauli(Pauli.PauliType.Y));
            q2.addGate(new Hadamard());

            qubits.Add(q1);
            qubits.Add(q2);

            return qubits;
        }


        [SetUp]
        public void Setup()
        {

        }

        /// <summary>
        /// Confirm that multiplying two Hadamard gates produces an expected output
        /// </summary>
        [Test]
        public void TestCalculation_HxH()
        {
            Matrix value = Matrix.Multiply(new Hadamard().getMatrix(), new Hadamard().getMatrix());

            Complex[,] data = new Complex[2, 2];
            data[0, 0] = 2;
            data[0, 1] = 0;
            data[1, 0] = 0;
            data[1, 1] = 2;

            Matrix expected = new Matrix(2, 2, -1, data);

            // Test the data as preceder value can be incorrect due to data rounding.
            Assert.AreEqual(expected.getData(), value.getData());
        }

        /// <summary>
        /// Confirm that Identity Matrices are correctly used to correct the size of a matrix for multiplication.
        /// </summary>
        [Test]
        public void TestCalculation_HxToffoli()
        {
            Matrix value = Matrix.Multiply(new Hadamard().getMatrix(), new Toffoli().getMatrix());

            Complex[,] data = new Complex[4, 4];
            data[0, 0] = 1;
            data[0, 2] = 1;
            data[1, 1] = 1;
            data[1, 3] = 1;
            data[2, 0] = 1;
            data[2, 2] = -1;
            data[3, 1] = 1;
            data[3, 3] = -1;

            Matrix expected = new Matrix(4, 4, -1, data);

            // Test the data as preceder value can be incorrect due to data rounding.
            Assert.AreEqual(expected.getData(), value.getData());

        }

        /// <summary>
        /// Test the multiplication of multiple matrices within gates on a qubit with no imaginary numbers
        /// </summary>
        [Test]
        public void TestSolveQubit1()
        {
            // NOTE: This is not how the circuit is solved, but a test of the maths for the solution process.

            Qubit qubit = createCircuit()[0]; // Get the first Qubit of the circuit     H    X    H

            Matrix value = qubit.getGates()[0].getMatrix();
            for (int i = 1; i < qubit.getGates().Count; i++)
            {
                value = Matrix.Multiply(value, qubit.getGates()[i].getMatrix());
            }

            Complex[,] data = new Complex[2, 2];
            data[0, 0] = 2;
            data[0, 1] = 0;
            data[1, 0] = 0;
            data[1, 1] = -2;

            Matrix expected = new Matrix(2, 2, -1, data);

            // Test the data as preceder value can be incorrect due to data rounding.
            Assert.AreEqual(expected.getData(), value.getData());
        }

        /// <summary>
        /// Test the multiplication of multiple matrices within gates on a qubit including imaginary numbers
        /// </summary>
        [Test]
        public void TestSolveQubit2()
        {

            Qubit qubit = createCircuit()[1]; // Get the second Qubit of the circuit     H    Y    H

            Matrix value = qubit.getGates()[0].getMatrix();
            for (int i = 1; i < qubit.getGates().Count; i++)
            {
                value = Matrix.Multiply(value, qubit.getGates()[i].getMatrix());
            }

            Complex[,] data = new Complex[2, 2];
            data[0, 0] = 0;
            data[0, 1] = new Complex(0, 2);
            data[1, 0] = new Complex(0, -2);
            data[1, 1] = 0;

            Matrix expected = new Matrix(2, 2, -1, data);

            // Test the data as preceder value can be incorrect due to data rounding.
            Assert.AreEqual(expected.getData(), value.getData());
        }

        /// <summary>
        /// Test the function of the Tensor Product on gates of matching size
        /// </summary>
        [Test]
        public void TestTensorProduct()
        {

            Matrix value = Matrix.Tensor(new Hadamard(), new Pauli(Pauli.PauliType.Z));

            Complex[,] data = new Complex[4, 4];
            data[0, 0] = 1;
            data[0, 2] = 1;
            data[1, 1] = -1;
            data[1, 3] = -1;
            data[2, 0] = 1;
            data[2, 2] = -1;
            data[3, 1] = -1;
            data[3, 3] = 1;

            Matrix expected = new Matrix(4, 4, -1, data);

            // Test the data as preceder value can be incorrect due to data rounding.
            Assert.AreEqual(expected.getData(), value.getData());
        }

        /// <summary>
        /// Test the function of the Tensor Product on gates of different sizes size
        /// </summary>
        [Test]
        public void TestTensorProductDifferentSizes()
        {

            Matrix value = Matrix.Tensor(new Hadamard(), new Toffoli());

            Complex[,] data = new Complex[8, 8];
            data[0, 0] = 1;
            data[0, 4] = 1;
            data[1, 1] = 1;
            data[1, 5] = 1;
            data[2, 2] = 1;
            data[2, 6] = 1;
            data[3, 3] = 1;
            data[3, 7] = 1;
            data[4, 4] = -1;
            data[4, 0] = 1;
            data[5, 5] = -1;
            data[5, 1] = 1;
            data[6, 6] = -1;
            data[6, 2] = 1;
            data[7, 7] = -1;
            data[7, 3] = 1;

            Matrix expected = new Matrix(4, 4, -1, data);

            // Test the data as preceder value can be incorrect due to data rounding.
            Assert.AreEqual(expected.getData(), value.getData());
        }

    }
}