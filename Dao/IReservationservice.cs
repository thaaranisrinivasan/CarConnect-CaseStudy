using System.Collections.Generic;
using CarConnectEntityLibrary;

namespace CarConnectDaoLibrary
{
    public interface IReservationService
    {
        Reservation GetReservationById(int reservationId);
        List<Reservation> GetReservationsByCustomerId(int customerId);
        void CreateReservation(Reservation reservation);
        void UpdateReservation(Reservation reservation);
        void CancelReservation(int reservationId);
    }
}
