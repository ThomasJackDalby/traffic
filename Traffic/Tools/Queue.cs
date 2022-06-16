//namespace Traffic
//{
//    public class Queue<T>
//    {
//        public int Length => front > back ? front - back; // not entirely true

//        private int front = 0;
//        private int back = 0;
//        private T[] contents = new T[4];
//        // if front == back (queue is empty)

//        public void Enqueue(T value)
//        {
//            contents[back++ % contents.Length] = value;
//            if (front == back)
//            {
//                T[] target = new T[contents.Length * 2];
//                for(int i = 0; i < contents.Length; i++) target[i] = contents[(front + i % contents.Length)];
//                front = 0;
//                back = contents.Length;
//                contents = target;
//            }
//        }
//        public T Peek()
//        {
//            if (front == back) throw new Exception();
//            return contents[front % contents.Length];
//        }
//        public T Dequeue()
//        {
//            if (front == back) throw new Exception();
//            return contents[front++ % contents.Length];
//        }
//    }
//}

//// [-1, -1, -1, -1] 0 0
//// [1, -1, -1, -1] 0 1
//// [1, 2, -1, -1] 0 2
//// [?, 2, -1, -1] 1 2
