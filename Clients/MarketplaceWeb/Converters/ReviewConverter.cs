using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MarketplaceWeb.Models;
using VirtoCommerce.ApiClient.DataContracts;

namespace MarketplaceWeb.Converters
{
    public static class ReviewConverter
    {
        public static ReviewModel ToWebModel(this Review review)
        {
            var retVal = new ReviewModel
            {
                Created = review.Created,
                Id = review.Id,
                Rating = review.Rating,
                ReviewText = review.ReviewText,
                Title = review.RatingComment,
                Author = new User
                {
                    Id = review.AuthorId,
                    Name = review.AuthorName,
                    Icon = @"/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAkGBwgHBgkIBwgKCgkLDRYPDQwMDRsUFRAWIB0iIiAdHx8kKDQsJCYxJx8fLT0tMTU3Ojo6Iys/RD84QzQ5OjcBCgoKDQwNGg8PGjclHyU3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3N//AABEIAHcAeAMBIgACEQEDEQH/xAAcAAEBAQADAQEBAAAAAAAAAAAABwYBBAUIAwL/xAA9EAABAwICBgcEBgsAAAAAAAAAAQIDBBEFBgcSITFBgRdRVWGRlNITQnGhFCIjMmKxFSQzUlRyc6LB0fD/xAAUAQEAAAAAAAAAAAAAAAAAAAAA/8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAwDAQACEQMRAD8A7Gd84V2J4lPSUVRJBQQvWNrY3K1ZbbFc5U3ovBN1jHgAAAAAAAAAAAAAAAAAbDI+cK3DMSgpK2oknoJnoxzZXK5Yrrsc1V3J1puBj924AAAAAAAA7WF4dVYrXRUVBEsk8i7E4InFVXgneB1QWHAtGuFUcbH4prV9RvVFVWxovc1N/PwPfXKeXlZqrgtBb+g2/iB8/grGYdGdDPE+XA3LSzptSF7ldG7uuu1v5dxLKumno6mSmqonRTxO1Xscm1FA/IAAAAAAAAAAAAAK9omwiOmwR2Juaiz1j1RHKm1sbVsic1RV8OokJe8iWTKGFW/h0A94AACcaXsGjdSQYzE1EljckMy/vNX7q8l2cyjmV0nOamS67WS93woid/tGgRAAAAAAAAAAAAAALVotrkq8pww615KWR8TvhfWT5OTwIqbHRjjrcKxtaSodamrrMuu5sifdXne3NALQDhDkAYDTBXJFgtJRIqa9RPrqn4WJ/tUN8q2ITn3G245mGWWF2tSwJ7GBU3ORF2u5rflYDOgAAAAAAAAAAAERVVERFVVWyInEAdmjw+tr7pRUdRUW3+xic63NCiZO0dNWNlbmJiq5yXZR3sifz9a/h8SkQQx08TYoI2RxtSzWMajURO5EAyGQs1/pKH9F4q5Y8Vp/qqkv1XTInGy+91pz+GzPAzLlOgx9qSS61PWs/Z1UWx7bbr9af8ioZ5uUc1yfqtRmuT6HuVzdb2ip8l/uUD+tIOaXIx+A4Jrz186K2ZYEVzo28Wpb3l+SEoqKeekl9jVQSQSJ7kjFavgpfcvZbw3L8GpQRfaOT7Sd+17/AIr/AITYd3EMOo8Tp1p6+miniX3ZG3t8Ope9APnEG/zho8loGSVuB689O3a+mct3sTravvJ3b/iYAAAAAAAAAAUnRZlhstsdrmXRHKlIxycU3v8AHYnPuJta+xF1b7LrwLRQ55ylQUUFJT1z2xQRtjYn0aXciWT3QNgcmV6RMr9oP8tL6R0iZX7Qf5WX0gaoGV6RMr9oP8rL6R0iZX7Qf5aX0gaoGV6RMr9oP8tL6R0iZX7Qf5aX0galSR6Tsrsw2oTFqCNG0079WdjU2RyLxTuX8/ibPpEyv2g/ysvpOljGc8pYthlTQT4g/UnjVmt9Fl+qvBfu8FsvICOgJy5AAAAAOXNVrla5FRUWyovA4AAAAAAAAAAAAAAAAAA5a1XORrUuqrZE6wBv89ZHqoa2oxPCmNkpZXLJLHrI10Tl32vvS+3rJ+uxbLvAAAAAAAAAAAAAAAATatk3gAUDIeR6qatp8UxVjY6WJySRR6yOWVyblW25L81AAH//2Q=="
                }
            };

            return retVal;
        }
    }
}