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
        void GetUsersMainPageInfo(User user);

        
        void Create(User user);
        void Update(User user);
        void Remove(User user);
        Like GetLikeById(int id);
        Comment GetCommentById(int id);
        Post GetPostById(int id);
        Message GetMessageById(int id);

        void Create(Dialog dialog);
        void Remove(Dialog dialog);
        void Update(Dialog dialog);


        User GetUserById(int id);
        HashSet<Like> GetUsersLikes(User user);


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


        List<Dialog> GetDialogs(User user);
        List<Message> GetMessagesFromDialog(Dialog dialog);

        void Save();

        void ClearData();

    }
}