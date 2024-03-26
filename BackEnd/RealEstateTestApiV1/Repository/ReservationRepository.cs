﻿using Microsoft.EntityFrameworkCore;
using RealEstateTestApi.Data;
using RealEstateTestApi.IRepository;
using RealEstateTestApi.Models;

namespace RealEstateTestApi.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private SWPRealEstateContext swpRealEstateContext;
        public ReservationRepository(SWPRealEstateContext swpRealEstateContext)
        {
            this.swpRealEstateContext = swpRealEstateContext;
        }

        public Reservation CreateReservation(Reservation reservation)
        {
            swpRealEstateContext.Reservations.Add(reservation);
            swpRealEstateContext.SaveChanges();
            return reservation;
        }

        public Reservation FindByID(int id)
        {
            Reservation re = swpRealEstateContext.Reservations.Find(id);
            if (re == null)
            {
                return null;
            }
            return re;
        }

        public List<Reservation> GetAllReservation()
        {
            List<Reservation> list = new List<Reservation>();
            list = swpRealEstateContext.Reservations.ToList();
            return list;
        }

        public Reservation UpdateReservation(Reservation reservation)
        {
            swpRealEstateContext.Reservations.Update(reservation);
            swpRealEstateContext.SaveChanges();
            return reservation;
        }

        //26_03
        public List<Reservation> GetAllReservationByAgencyId(int id)
        {
            List<Reservation> list = swpRealEstateContext.Reservations.Where(x=>x.AgencyId == id).ToList();
            return list;
        }

        public List<Reservation> GetAllReservationByCustomerId(int id)
        {
            List<Reservation> list = swpRealEstateContext.Reservations.Where(x => x.CustomerId == id).ToList();
            return list;
        }

    }
}
