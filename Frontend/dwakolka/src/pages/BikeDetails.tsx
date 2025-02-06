import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Value } from "react-calendar/dist/cjs/shared/types";
import axios from "axios";
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";
import "./BikeDetails.css";

import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer } from "recharts";
import { time } from "console";

interface RentalPeriod {
  startTime: string;
  endTime: string;
}





interface Bike {
  IDbike: number;
  nameModel: string;
  typeModel: string;
  priceModel: number;
  photoModel: string;
  availableBike: boolean;
  IDreturnLocation?: string;
}

const API_URL = localStorage.getItem("API_URL");

const BikeDetails = () => {
  const { id } = useParams<{ id: string }>();
  const [bike, setBike] = useState<Bike | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [rentalPeriods, setRentalPeriods] = useState<RentalPeriod[]>([]);
  const [reservationSuccess, setReservationSuccess] = useState<boolean | null>(null);

  useEffect(() => {
    axios
      .get(`${API_URL}/Bikes/${id}`)
      .then((response) => {
        //console.log(response.data);
        setBike(response.data);
        setRentalPeriods(response.data.rentalPeriods);
        setLoading(false);
      })
      .catch(() => {
        setError("Failed to load bike details.");
        setRentalPeriods([]);
        setLoading(false);
      });
  }, [id]);

  const validRentalPeriods = rentalPeriods.filter(period => period && period.startTime && period.endTime);

  const isDateBooked = (date: Date) => {
    if (!bike?.availableBike) return true;
    return validRentalPeriods.some((period) => {
      const periodStart = new Date(period.startTime);
      const periodEnd = new Date(period.endTime);
      return date >= periodStart && date <= periodEnd;
    });
  };
/*
  const handleCalendarChange = (value: Date | Date[] | null) => {
    if (Array.isArray(value)) {
      const [start, end] = value;
      setStartDate(start.toISOString().split("T")[0]);
      setEndDate(end.toISOString().split("T")[0]);
    }
  };*/

  const handleCalendarChange = (value: Value) => {
    if (Array.isArray(value)) {
      const [start, end] = value;
      if(start != null && end != null){
      setStartDate(start.toISOString().split("T")[0]);
      setEndDate(end.toISOString().split("T")[0]);}
    } else if (value instanceof Date) {
      setStartDate(value.toISOString().split("T")[0]);
      setEndDate(value.toISOString().split("T")[0]);
    } else {
      // Handle null case if needed
      setStartDate("");
      setEndDate("");
    }
  };


// Convert data for visualization
const formattedData = validRentalPeriods.map((period) => ({
  name: new Date(period.startTime).toLocaleDateString(),
  duration: (new Date(period.endTime).getTime() - new Date(period.startTime).getTime()) / (1000 * 60 * 60 * 24),
  endDate: new Date(period.endTime).toLocaleDateString(),
}));

  const handleReserve = () => {
    const userId = localStorage.getItem("userId");

    if (!userId) return alert("You must be logged in to reserve a bike.");
    if (!startDate || !endDate) return alert("Please select start and end dates.");
    if (endDate < startDate) return alert("End date cannot be before start date!");
    const today = new Date().toISOString().split("T")[0];
    if (startDate < today) return alert("Start date cannot be before today!");

    const selectedStart = new Date(startDate);
    const selectedEnd = new Date(endDate);
  
    // Check for overlaps
    const isOverlapping = rentalPeriods.some(period => {
      if (!period || !period.startTime || !period.endTime) return false; // Skip invalid entries
  
      const periodStart = new Date(period.startTime);
      const periodEnd = new Date(period.endTime);
  
      // Check if the selected period overlaps with any existing rental period
      return selectedStart < periodEnd && selectedEnd > periodStart;
    });

    if (isOverlapping) {
      alert("The selected reservation period overlaps with an existing reservation. Please choose a different time.");
      return; // Stop the reservation process
    }

    const daysReserved = Math.ceil((selectedEnd.getTime() - selectedStart.getTime()) / (1000 * 60 * 60 * 24));
    var totalPrice = 0;
    if (bike != null){
      totalPrice = daysReserved * bike.priceModel;}
    
    const confirmReservation = window.confirm(
      `The total price for this reservation is $${totalPrice}. Do you want to proceed?`
    );
  
    if (!confirmReservation) return; // Stop reservation if user cancels
  

    axios
      .post(`${API_URL}/Reservation/reserve`, {
        id: userId,
        bikeId: id,
        startTime: startDate,
        endTime: endDate,
      })
      .then(() => {
        setReservationSuccess(true);
        setRentalPeriods(prevPeriods => [
          ...prevPeriods,
          { startTime: startDate, endTime: endDate }
        ]);
      })
      .catch(() => setReservationSuccess(false));

      
  };

  if (loading) return <p className="loading-message">Loading bike details...</p>;
  if (error) return <p className="error-message">{error}</p>;
  if (!bike) return <p className="loading-message">No bike found.</p>;
 // console.log(rentalPeriods)
  return (
    <div className="bike-details-container">
      <h2 className="bike-title">{bike.nameModel}</h2>

      <div className="bike-details-grid">
        <div className="bike-image-container">
          <img src={bike.photoModel} alt="Bike" className="bike-image" />
        </div>

        <div className="bike-info">
          <p className="bike-type">{bike.typeModel}</p>
          <p className="bike-price">${bike.priceModel}</p>
          <p className={`bike-availability ${bike.availableBike ? "available" : "not-available"}`}>
            {bike.availableBike ? "Available" : "Not Available"}
          </p>
          <p className="bike-location">Return Location: {bike.IDreturnLocation || "Unknown"}</p>

          {/**/}
          <div className="reservation-form">
            <h3 className="form-title">Reserve this Bike</h3>

            <label>Start Date:</label>
            <input
              type="date"
              value={startDate}
              onChange={(e) => setStartDate(e.target.value)}
              className="date-input"
            />

            <label>End Date:</label>
            <input
              type="date"
              value={endDate}
              onChange={(e) => setEndDate(e.target.value)}
              className="date-input"
            />

            <button onClick={handleReserve} className="reserve-button" disabled={!bike.availableBike}
              title={!bike.availableBike ? "This bike is currently unavailable for reservation." : ""}>
              Reserve Now
            </button>

            <h3>Reserved Time Periods</h3>
            

            {reservationSuccess && <p className="success-message">Bike Reserved Successfully!</p>}
            {reservationSuccess === false && <p className="error-message">Failed to Reserve. Try Again.</p>}
          </div>
          <div>
          <Calendar
              minDate={new Date()}
              selectRange={true}
              onChange={handleCalendarChange}
              tileClassName={({ date, view }) => {
                if (view !== "month") return null;
                return isDateBooked(date) ? "unavailable" : "available";
              }}
            />
          </div>
        </div>
      </div>
    </div>
  );
};
/*{validRentalPeriods.length === 0 ? (
  <p>No known reservation periods for this bike.</p>

  
) : (
    <ResponsiveContainer width="100%" height={300}>
      <BarChart data={formattedData}>
        <XAxis dataKey="name" />
        <YAxis />
        <Tooltip 
      content={({ payload }) => {
        if (payload && payload.length) {
          const { name, duration, endDate } = payload[0].payload;
          return (
            <div className="custom-tooltip">
              <p><strong>Start Date:</strong> {name}</p>
              <p><strong>Duration:</strong> {duration} days</p>
              <p><strong>End Date:</strong> {endDate}</p>
            </div>
          );
        }
        return null;
      }}
    />
        <Bar dataKey="duration" fill="#8884d8" />
      </BarChart>
    </ResponsiveContainer>
  )}*/
export default BikeDetails;
