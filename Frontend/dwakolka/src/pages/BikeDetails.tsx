import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";

interface Bike {
  IDbike: number;
  nameModel: number;
  typeModel: string;
  priceModel: number;
  photoModel: string;
  availableBike: boolean;
  IDreturnLocation?: string;
}

const BikeDetails = () => {
  var { id } = useParams<{ id: string }>();
  const [bike, setBike] = useState<Bike | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [startDate, setStartDate] = useState<string>("");
  const [endDate, setEndDate] = useState<string>("");
  //const [userId] = localStorage.getItem("userId"); // Simulated logged-in user ID
  const [reservationSuccess, setReservationSuccess] = useState<boolean | null>(null);

  useEffect(() => {
    axios.get(`http://localhost:5008/api/Bikes/${id}`)
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

    if (!userId) {
      alert("You must be logged in to reserve a bike.");
      return;
    }

    if (!startDate || !endDate) {
      alert("Please select start and end dates.");
      return;
    }

    if (endDate < startDate){
        alert("End date cannot be before start date!");
        return;
    }

    const endTime = endDate;
    const startTime = startDate;
    const iDpayment:number = 12345;
    var temp = id;
    id = userId;
    const bikeId = temp;
    axios.post("http://localhost:5008/api/Reservation/reserve", {
      id ,
      bikeId,
      
      startTime,
      endTime
    })
    .then(() => {
      setReservationSuccess(true);
    })
    .catch(() => {
      setReservationSuccess(false);
    });
    id = temp;
  };

  if (loading) return <p className="text-center text-gray-500">Loading bike details...</p>;
  if (error) return <p className="text-center text-red-500">{error}</p>;

  if (!bike) return <p className="text-center text-gray-500">No bike found.</p>;

  return (
    <div className="max-w-3xl mx-auto mt-10 p-6 border rounded-lg shadow-md bg-white">
      <h2 className="text-2xl font-bold text-center mb-4">Bike Details</h2>
      <img src={bike.photoModel} alt="Bike" className="w-full h-60 object-cover rounded-md mb-4"/>
      <p className="text-lg font-semibold">Model {bike.nameModel}</p>
      <p className="text-gray-600">{bike.typeModel}</p>
      <p className="font-bold text-blue-600">${bike.priceModel}</p>
      <p className={`mt-2 text-sm ${bike.availableBike ? "text-green-500" : "text-red-500"}`}>
        {bike.availableBike ? "Available" : "Not Available"}
      </p>
      <p className="text-gray-500 text-sm">Return Location: {bike.IDreturnLocation || "Unknown"}</p>

      {/* Reservation Form */}
      <div className="mt-6">
        <label className="block mb-1 text-sm font-semibold">Start Date:</label>
        <input
          type="date"
          value={startDate}
          onChange={(e) => setStartDate(e.target.value)}
          className="border rounded p-2 w-full mb-3"
        />

        <label className="block mb-1 text-sm font-semibold">End Date:</label>
        <input
          type="date"
          value={endDate}
          onChange={(e) => setEndDate(e.target.value)}
          className="border rounded p-2 w-full mb-3"
        />

        <button
          onClick={handleReserve}
          className="mt-3 bg-blue-500 text-white py-2 px-6 rounded hover:bg-blue-600 transition w-full"
        >
          Reserve Bike
        </button>

        {reservationSuccess === true && <p className="text-green-600 mt-3">Bike Reserved Successfully!</p>}
        {reservationSuccess === false && <p className="text-red-600 mt-3">Failed to Reserve. Try Again.</p>}
      </div>
    </div>
  );
};

export default BikeDetails;
