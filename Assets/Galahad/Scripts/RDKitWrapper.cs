using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Galahad.Scripts
{
    internal class RDKitWrapper
    {
        [DllImport("RDKitWrapper")]
        public static extern int DrawSVG(string smiles, [Out] StringBuilder buf, int bufsize);

        [DllImport("RDKitWrapper")]
        public static extern int OptFromSmiles(
            string smiles,
            [Out] int[] atomicNums,
            [Out] int[] atomCharges,
            [Out] double[] positions,
            [Out] int[] bondConnections,
            [Out] double[] bondOrders,
            [Out] int[] numAtoms,
            [Out] int[] numBonds
        );

        [DllImport("RDKitWrapper")]
        public static extern int OptMolecule(
            [In] int numAtoms,
            [In] int[] atomicNums,
            [In] int[] atomCharges,
            [In] int numBonds,
            [In] int[] bondConnections,
            [In] double[] bondOrders,
            [Out] double[] positions
        );

        [DllImport("RDKitWrapper")]
        public static extern int ReadSdf(
            [In] string path,
            [Out] int[] numMolecules,
            [Out] int[] numAtoms,
            [Out] int[] atomicNums,
            [Out] int[] atomCharges,
            [Out] int[] numBonds,
            [Out] int[] bondConnections,
            [Out] double[] bondOrders,
            [Out] double[] positions
        );
    }
}