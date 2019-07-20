namespace MovieDatabase.Common
{
    public class ValidationConstants
    {
        #region Announcement
        public const int announcementTitleMinimumLength = 5;
        public const int announcementTitleMaximumLength = 200;

        public const int announcementCreatorMinimumLength = 3;
        public const int announcementCreatorMaximumLength = 50;

        public const int announcementContentMinimumLength = 10;
        public const int announcementContentMaximumLength = 1000;
        #endregion

        #region Artist
        public const int artistFullNameMinimumLength = 3;
        public const int artistFullNameMaximumLength = 50;

        public const int artistBiographyMinimumLength = 25;
        public const int artistBiographyMaximumLength = 10000;
        #endregion

        #region Avatar
        public const int userAvatarMaximumFileSizeInBytes = 64000;
        #endregion

        #region Genre
        public const int genreNameMinimumLength = 3;
        public const int genreNameMaximumLength = 30;
        #endregion

        #region Movie
        public const int movieNameMinimumLength = 3;
        public const int movieNameMaximumLength = 50;

        public const int movieDescriptionMinimumLength = 25;
        public const int movieDescriptionMaximumLength = 1000;

        public const int movieMinimumLengthInMinutes = 60;
        public const int movieMaximumLengthInMinutes = 300;
        #endregion

        #region Review
        public const int reviewContentMinimumLength = 10;
        public const int reviewContentMaximumLength = 10000;

        public const int reviewMinimumRating = 1;
        public const int reviewMaximumRating = 10;
        #endregion

        #region Role
        public const int roleCharacterPlayedMinimumLength = 3;
        public const int roleCharacterPlayedMaximumLength = 30;
        #endregion

        #region Season
        public const int seasonMinimumSeasonNumber = 1;
        public const int seasonMaximumSeasonNumber = 2147483647;

        public const int seasonMinimumEpisodes = 3;
        public const int seasonMaximumEpisodes = 44;

        public const int seasonMinimumLengthPerEpisode = 20;
        public const int seasonMaximumLengthPerEpisode = 120;
        #endregion

        #region TV Show
        public const int tvShowNameMinimumLength = 3;
        public const int tvShowNameMaximumLength = 50;

        public const int tvShowDescriptionMinimumLength = 25;
        public const int tvShowDescriptionMaximumLength = 1000;
        #endregion
    }
}