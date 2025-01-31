import { useEffect, useState } from "react";
import axios from "axios";

interface Reservation {
  id: string;
  bikeId: number;
  nameModel: number;
  typeModel: string;
  priceModel: number;
  photoModel: string;
  startDate: string;
  endDate: string;
}

const MyReservations = () => {
  const [reservations, setReservations] = useState<Reservation[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const userId = "123e4567-e89b-12d3-a456-426614174000"; // Simulated logged-in user ID

  useEffect(() => {
    axios.get(`http://localhost:5008/api/reservations/${userId}`)
      .then((response) => {
        setReservations(response.data);
        setLoading(false);
      })
      .catch(() => {
        setError("Failed to load reservations.");
        setLoading(false);
      });
  }, [userId]);

  if (loading) return <p className="text-center text-gray-500">Loading reservations...</p>;
  if (error) return <p className="text-center text-red-500">{error}</p>;

  return (
    <div className="max-w-6xl mx-auto mt-10">
      <h2 className="text-2xl font-bold text-center mb-6">My Reservations</h2>
      {reservations.length === 0 ? (
        <p className="text-center text-gray-500">You have no reservations.</p>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
          {reservations.map((res) => (
            <div key={res.id} className="border p-4 rounded-lg shadow-md bg-white">
              <img src={res.photoModel} alt="Bike" className="w-full h-40 object-cover rounded-md mb-3"/>
              <h3 className="text-lg font-semibold">Model {res.nameModel}</h3>
              <p className="text-gray-600">{res.typeModel}</p>
              <p className="font-bold text-blue-600">${res.priceModel}</p>
              <p className="text-sm text-gray-500 mt-2">Start: {new Date(res.startDate).toLocaleDateString()}</p>
              <p className="text-sm text-gray-500">End: {new Date(res.endDate).toLocaleDateString()}</p>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default MyReservations;
