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
            Hadamard H = new Hadamard();

            Matrix M = H.getMatrix();
            Matrix value = Matrix.Multiply(M, M);

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
    }
}