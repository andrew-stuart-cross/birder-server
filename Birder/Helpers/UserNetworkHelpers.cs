﻿using Birder.Data.Model;
using Birder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Birder.Helpers
{
    public static class UserNetworkHelpers
    {
        public static List<string> GetFollowersUserNames(ICollection<Network> followers)
        {
            if (followers == null)
                throw new NullReferenceException("The followers collection is null");

            return (from user in followers
                    select user.Follower.UserName).ToList();
        }

        public static List<string> GetFollowingUserNames(ICollection<Network> following)
        {
            if (following == null)
                throw new NullReferenceException("The following collection is null");

            return (from user in following
                    select user.ApplicationUser.UserName).ToList();
        }

        public static IEnumerable<string> GetFollowersNotBeingFollowedUserNames(ApplicationUser loggedinUser)
        {
            if (loggedinUser == null)
                throw new NullReferenceException("The user is null");

            List<string> followersUsernamesList = GetFollowersUserNames(loggedinUser.Followers);
            List<string> followingUsernamesList = GetFollowingUserNames(loggedinUser.Following);
            followingUsernamesList.Add(loggedinUser.UserName); //include own user name
            
            return followersUsernamesList.Except(followingUsernamesList);
        }

        public static bool UpdateIsFollowing(string requestingUsername, ICollection<Network> requestedUsersFollowing)
        {
            if (string.IsNullOrEmpty(requestingUsername))
                throw new ArgumentNullException(nameof(requestingUsername), "The requestingUsername is null");

            if (requestedUsersFollowing == null)
                throw new ArgumentNullException(nameof(requestedUsersFollowing), "The following collection is null");

            return requestedUsersFollowing.Any(cus => cus.ApplicationUser.UserName == requestingUsername);
        }

        public static bool UpdateIsFollowingProperty(string requestingUsername, ICollection<Network> requestedUsersFollowers)
        {
            if(string.IsNullOrEmpty(requestingUsername))
                throw new ArgumentNullException(nameof(requestingUsername), "The requestingUsername is null");

            if(requestedUsersFollowers == null)
                throw new ArgumentNullException(nameof(requestedUsersFollowers), "The followers collection is null");

            return requestedUsersFollowers.Any(cus => cus.Follower.UserName == requestingUsername);
        }

        /// <summary>
        /// Updates the requesting user's 'following' network
        /// </summary>
        /// <param name="requestingUser"></param>
        /// <param name="following"></param>
        /// <returns></returns>
        public static IEnumerable<FollowingViewModel> SetupFollowingCollection(ApplicationUser requestingUser, IEnumerable<FollowingViewModel> following)
        {
            if (following == null)
                throw new ArgumentNullException(nameof(following), "The following collection is null");

            if (requestingUser == null)
                throw new ArgumentNullException(nameof(requestingUser), "The requesting user is null");

            for (int i = 0; i < following.Count(); i++)
            {
                following.ElementAt(i).IsFollowing = requestingUser.Following.Any(cus => cus.ApplicationUser.UserName == following.ElementAt(i).UserName);
                following.ElementAt(i).IsOwnProfile = following.ElementAt(i).UserName == requestingUser.UserName;
            }

            return following;
        }


        public static IEnumerable<FollowerViewModel> SetupFollowersCollection(ApplicationUser requestingUser, IEnumerable<FollowerViewModel> followers)
        {
            if (followers == null)
                throw new ArgumentNullException(nameof(followers), "The followers collection is null");

            if (requestingUser == null)
                throw new ArgumentNullException(nameof(requestingUser), "The requesting user is null");

            for (int i = 0; i < followers.Count(); i++)
            {
                followers.ElementAt(i).IsFollowing = requestingUser.Following.Any(cus => cus.ApplicationUser.UserName == followers.ElementAt(i).UserName);
                followers.ElementAt(i).IsOwnProfile = followers.ElementAt(i).UserName == requestingUser.UserName;
            }

            return followers;
        }
    }
}
