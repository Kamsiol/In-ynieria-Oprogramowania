import { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import "./BikeList.css"; // Подключаем CSS

interface Bike {
  iDbike: number;
  iDmodel: number;
  nameModel: string;
  typeModel: string;
  priceModel: number;
  amountBike: number;
  availableBike: boolean;
}

const API_URL = localStorage.getItem("API_URL");

const BikeList = () => {
  const [bikes, setBikes] = useState<Bike[]>([]);
  const [filteredBikes, setFilteredBikes] = useState<Bike[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  // Фильтры
  const [searchQuery, setSearchQuery] = useState<string>("");
  const [selectedAvailability, setSelectedAvailability] = useState<string>("All");

  useEffect(() => {
    axios
      .get(`${API_URL}/Bikes`)
      .then((response) => {
        setBikes(response.data);
        setFilteredBikes(response.data);
        setLoading(false);
      })
      .catch(() => {
        setError("Failed to load bikes.");
        setLoading(false);
      });
  }, []);

  // Фильтрация списка велосипедов
  useEffect(() => {
    let filtered = bikes;

    if (selectedAvailability !== "All") {
      const isAvailable = selectedAvailability === "Available";
      filtered = filtered.filter((bike) => bike.availableBike === isAvailable);
    }

    if (searchQuery) {
      filtered = filtered.filter((bike) =>
        bike.nameModel.toLowerCase().includes(searchQuery.toLowerCase())
      );
    }

    setFilteredBikes(filtered);
  }, [searchQuery, selectedAvailability, bikes]);

  if (loading) return <p className="loading-message">Loading bikes...</p>;
  if (error) return <p className="error-message">{error}</p>;

  return (
    <div className="bike-list-container">
      <h2 className="bike-list-title">Available Bikes</h2>

      {/* Фильтры */}
      <div className="filters">
        <input
          type="text"
          placeholder="Search by model..."
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
          className="search-input"
        />

        <select
          value={selectedAvailability}
          onChange={(e) => setSelectedAvailability(e.target.value)}
          className="filter-select"
        >
          <option value="All">All Availability</option>
          <option value="Available">Available</option>
          <option value="Not Available">Not Available</option>
        </select>
      </div>

      {/* Сетка велосипедов */}
      <div className="bike-grid">
        {filteredBikes.length > 0 ? (
          filteredBikes.map((bike) => (
            <div key={bike.iDbike} className="bike-card">
              <h3 className="bike-name">Model {bike.nameModel}</h3>
              <p className="bike-type">{bike.typeModel}</p>
              <p className="bike-price">${bike.priceModel}</p>
              <p className={`bike-availability ${bike.availableBike ? "available" : "not-available"}`}>
                {bike.availableBike ? "Available" : "Not Available"}
              </p>
              <button
                onClick={() => navigate(`/bike/${bike.iDbike}`)}
                className="details-button"
              >
                View Details
              </button>
            </div>
          ))
        ) : (
          <p className="no-results">No bikes match your filters.</p>
        )}
      </div>
    </div>
  );
};

export default BikeList;
