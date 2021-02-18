using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmVisualization
{
    public class LostOrdering
    {
        public static string userNameAsSHA(string userName)
        {
            string hashedString = userName.ToLower();
            int indexAt = hashedString.IndexOf('@');
            if (indexAt > 0) hashedString = hashedString.Substring(0, indexAt);
            if (hashedString.Length < 10) hashedString += "1234567879012354";
            hashedString = hashedString.Substring(0, 10);

            return sha256_hash(hashedString);
        }

        public static string sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public static Type[] SetupLostSortOrdering(string sha)
        {
            // Get the count of lost sorts
            var listOfBs = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                            from assemblyType in domainAssembly.GetTypes()
                            where typeof(Algorithms.AbstractAlgorithm).IsAssignableFrom(assemblyType)
                            select assemblyType).ToArray();

            int count = 0;
            List<Type> lostAlgorithms = new List<Type>();
            foreach (var subclass in listOfBs)
            {
                if (!subclass.IsAbstract)
                {
                    Algorithms.AbstractAlgorithm tempAlgorithm = 
                        (Algorithms.AbstractAlgorithm)Activator.CreateInstance(subclass);
                    if (tempAlgorithm.GenerateLostSort)
                    {
                        lostAlgorithms.Add(subclass);
                        count++;
                    }
                }
            }
            List<int> order = new List<int>();

            // Normalize the sha string
            for (int i = 0; i < sha.Length; i++)
            {
                int value = Convert.ToInt32("" + sha[i], 16) % count;
                if (!order.Contains(value))
                {
                    order.Add(value);
                }
            }

            //Ensure all of the values exist in order
            for (int i = 0; i < count; i++)
            {
                if (!order.Contains(i)) order.Add(i);
            }

            Type[] orderedAlgorithms = new Type[count];
            for (int i = 0; i < count; i++)
            {
                orderedAlgorithms[i] = lostAlgorithms[order[i]];
            }
            return orderedAlgorithms;
        }

    }
}
