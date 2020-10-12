using System;
using System.Collections.Generic;

namespace SocialNetwork.Models
{
    public interface IUsersRepository
    {
        IEnumerable<User> Users { get; }
        IEnumerable<Post> Posts { get; }
        IEnumerable<Like> Likes { get; }
        IEnumerable<Comment> Comments { get; }
        IEnumerable<Message> Messages { get; }
        IEnumerable<Photo> Photos { get; }
        IEnumerable<Friendship> Friendships { get; }

        List<Post> GetUsersPosts(User user);
        List<Post> GetUsersPhotos(User user); 
        List<User> GetUsersFriends(User user);
        List<Post> GetUsersNews(User user);
      
        Post GetUsersMainPhoto(User user);

        User GetUserById(int id);

        void Create(Message message);

        void Create(Post post);
        void Remove(Post post);

        void Create(Like like);
        void Remove(Like like);

        void Create(Comment comment);
        void Remove(Comment comment);

        void Create(Photo photo);
        void Remove(Photo photo);

        void Create(Friendship friendship);
        void Update(Friendship friendship);
        void Remove(Friendship friendship);

        
        List<User> GetDialogs(User user);

        List<Message> GetUsersMessages(User user, User dialogUser);

        Message GetFirstMessage(User user, User userWith);

        void Save();

        void ClearData();

    }
}