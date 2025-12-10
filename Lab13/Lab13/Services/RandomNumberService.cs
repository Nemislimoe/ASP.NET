using System;

namespace Lab13.Services
{
    public class RandomNumberService : IRandomNumberService
    {
        private readonly Random _rnd = new Random();
        public int Next() => _rnd.Next(1, 101);
    }
}
