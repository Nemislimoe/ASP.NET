using Lab8.Models;
using System.Collections.Generic;
using System.Linq;

namespace Lab8.Data
{
    public class InMemoryStore
    {
        private readonly List<Post> _posts = new();
        private readonly List<TodoItem> _todos = new();
        private ContactForm _lastContact;

        private int _postNextId = 1;
        private int _todoNextId = 1;

        public InMemoryStore()
        {
            // початкові дані
            _posts.Add(new Post { Id = _postNextId++, Title = "Ласкаво просимо", Content = "Перший пост блогу.", Author = "Admin" });
            _posts.Add(new Post { Id = _postNextId++, Title = "Ще один пост", Content = "Приклад контенту.", Author = "Admin" });

            _todos.Add(new TodoItem { Id = _todoNextId++, Title = "Купити хліб", IsCompleted = false });
            _todos.Add(new TodoItem { Id = _todoNextId++, Title = "Підготувати лабораторку", IsCompleted = true });
        }

        // Posts
        public IEnumerable<Post> GetPosts() => _posts.OrderByDescending(p => p.Id);
        public Post GetPost(int id) => _posts.FirstOrDefault(p => p.Id == id);
        public Post AddPost(Post post)
        {
            post.Id = _postNextId++;
            _posts.Add(post);
            return post;
        }

        // Contact
        public void SaveContact(ContactForm contact) => _lastContact = contact;
        public ContactForm GetLastContact() => _lastContact;

        // Todos
        public IEnumerable<TodoItem> GetTodos() => _todos.OrderBy(t => t.Id);
        public TodoItem AddTodo(string title)
        {
            var todo = new TodoItem { Id = _todoNextId++, Title = title, IsCompleted = false };
            _todos.Add(todo);
            return todo;
        }
        public TodoItem ToggleTodo(int id)
        {
            var t = _todos.FirstOrDefault(x => x.Id == id);
            if (t != null) t.IsCompleted = !t.IsCompleted;
            return t;
        }
    }
}
