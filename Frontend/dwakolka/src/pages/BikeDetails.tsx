import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";
import "./BikeDetails.css"; // Подключаем стили

interface Bike {
  IDbike: number;
  nameModel: string;
  typeModel: string;
  priceModel: number;
  photoModel: string;
  availableBike: boolean;
  IDreturnLocation?: string;
}

const BikeDetails = () => {
  const { id } = useParams<{ id: string }>();
  const [bike, setBike] = useState<Bike | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [reservationSuccess, setReservationSuccess] = useState<boolean | null>(null);

  useEffect(() => {
    axios
      .get(`https://localhost:7057/api/Bikes/${id}`)
      .then((response) => {
        setBike(response.data);
        setLoading(false);
      })
      .catch(() => {
        setError("Failed to load bike details.");
        setLoading(false);
      });
  }, [id]);

  const handleReserve = () => {
    const userId = localStorage.getItem("userId");

    if (!userId) return alert("You must be logged in to reserve a bike.");
    if (!startDate || !endDate) return alert("Please select start and end dates.");
    if (endDate < startDate) return alert("End date cannot be before start date!");

    axios
      .post("https://localhost:7057/api/Reservation/reserve", {
        id: userId,
        bikeId: id,
        startTime: startDate,
        endTime: endDate,
      })
      .then(() => setReservationSuccess(true))
      .catch(() => setReservationSuccess(false));
  };

  if (loading) return <p className="loading-message">Loading bike details...</p>;
  if (error) return <p className="error-message">{error}</p>;
  if (!bike) return <p className="loading-message">No bike found.</p>;

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

          {/* Форма бронирования */}
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

            <button onClick={handleReserve} className="reserve-button">
              Reserve Now
            </button>

            {reservationSuccess && <p className="success-message">Bike Reserved Successfully!</p>}
            {reservationSuccess === false && <p className="error-message">Failed to Reserve. Try Again.</p>}
          </div>
        </div>
      </div>
    </div>
  );
};

export default BikeDetails;
