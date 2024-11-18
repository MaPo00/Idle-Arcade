using UnityEngine;

namespace Utils
{
    public static class Vectors
    {
        public static Vector3 GetRandomVectorAbove(Vector3 whereToStart, float yRandom, float xzRandom) =>
            whereToStart
            + Vector3.up * Random.Range(0, yRandom)
            + Vector3.left * (Random.Range(0, xzRandom) * Random.Range(-1f, 1f))
            + Vector3.forward * (Random.Range(0, xzRandom) * Random.Range(-1f, 1f));

        public static Vector3 GetRandomVectorRotation(float random) =>
            Vector3.up * Random.Range(0, random) * Random.Range(-1f, 1f)
            + Vector3.left * (Random.Range(0, random) * Random.Range(-1f, 1f))
            + Vector3.forward * (Random.Range(0, random) * Random.Range(-1f, 1f));
    }
}