import React, { useState, useEffect } from "react";
import {
  Container,
  Typography,
  Button,
  Box,
  Modal,
  TextField,
  MenuItem,
  Card,
  CardContent,
} from "@mui/material";
import axios from "axios";

const modalStyle = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  p: 4,
  borderRadius: 2,
};

export default function RiderDashboard() {
  const [open, setOpen] = useState(false);
  const [trips, setTrips] = useState([]);

  const [pickupLat, setPickupLat] = useState("");
  const [pickupLng, setPickupLng] = useState("");
  const [destLat, setDestLat] = useState("");
  const [destLng, setDestLng] = useState("");
  const [vehicleType, setVehicleType] = useState(0);

  const token = localStorage.getItem("token");

  // 🔹 Fetch trips when page loads
  useEffect(() => {
    fetchTrips();
  }, []);

  const fetchTrips = async () => {
    try {
      const response = await axios.get(
        "https://localhost:7044/api/trips/1/rider",
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      setTrips(response.data);
    } catch (error) {
      console.error("Error fetching trips", error);
    }
  };

  const updateTripStatus = async (tripId, action) => {
  try {
    await axios.post(
      `https://localhost:7044/api/trips/${tripId}/${action}`,
      {},
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    fetchTrips(); // refresh list
  } catch (error) {
    console.error("Error updating trip", error);
    alert("Failed to update trip");
  }
};


  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await axios.post(
        "https://localhost:7044/api/trips/create",
        {
          pickupLatitude: parseFloat(pickupLat),
          pickupLongitude: parseFloat(pickupLng),
          destinationLatitude: parseFloat(destLat),
          destinationLongitude: parseFloat(destLng),
          vehicleType: vehicleType,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      alert("Trip Requested Successfully 🚗");
      setOpen(false);
      fetchTrips(); // refresh list after creating trip
    } catch (error) {
      alert("Error requesting trip");
    }
  };

  return (
    <Container sx={{ mt: 8 }}>
      <Typography variant="h4" gutterBottom>
        Rider Dashboard
      </Typography>

      <Button variant="contained" onClick={() => setOpen(true)}>
        Request Trip
      </Button>

      {/* 🔹 Trip List */}
      {trips.map((trip) => (
  <Card key={trip.id} sx={{ mb: 2 }}>
    <CardContent>
      <Typography>
        <strong>Status:</strong> {trip.tripStatus}
      </Typography>

      <Typography>
        <strong>Fare:</strong> ${trip.fare}
      </Typography>

      <Typography>
        <strong>Pickup:</strong> {trip.pickupLatitude},{" "}
        {trip.pickupLongitude}
      </Typography>

      <Typography>
        <strong>Destination:</strong>{" "}
        {trip.destinationLatitude}, {trip.destinationLongitude}
      </Typography>

      <Box mt={2} display="flex" gap={1} flexWrap="wrap">
        {trip.tripStatus === 1 && (
          <>
            <Button
              size="small"
              variant="contained"
              onClick={() => updateTripStatus(trip.id, "accept")}
            >
              Accept
            </Button>

            <Button
              size="small"
              color="error"
              variant="outlined"
              onClick={() => updateTripStatus(trip.id, "cancel")}
            >
              Cancel
            </Button>
          </>
        )}

        {trip.tripStatus === 2 && (
          <>
            <Button
              size="small"
              variant="contained"
              onClick={() => updateTripStatus(trip.id, "arrived")}
            >
              Arrived
            </Button>

            <Button
              size="small"
              color="error"
              variant="outlined"
              onClick={() => updateTripStatus(trip.id, "cancel")}
            >
              Cancel
            </Button>
          </>
        )}

        {trip.tripStatus === 3 && (
          <Button
            size="small"
            variant="contained"
            onClick={() => updateTripStatus(trip.id, "start")}
          >
            Start Trip
          </Button>
        )}

        {trip.tripStatus === 4 && (
          <Button
            size="small"
            color="success"
            variant="contained"
            onClick={() => updateTripStatus(trip.id, "complete")}
          >
            Complete
          </Button>
        )}
      </Box>
    </CardContent>
  </Card>
))}


      {/* 🔹 Request Trip Modal */}
      <Modal open={open} onClose={() => setOpen(false)}>
        <Box sx={modalStyle}>
          <Typography variant="h6" mb={2}>
            Request a Trip
          </Typography>

          <Box component="form" onSubmit={handleSubmit}>
            <TextField
              label="Pickup Latitude"
              fullWidth
              margin="normal"
              value={pickupLat}
              onChange={(e) => setPickupLat(e.target.value)}
              required
            />

            <TextField
              label="Pickup Longitude"
              fullWidth
              margin="normal"
              value={pickupLng}
              onChange={(e) => setPickupLng(e.target.value)}
              required
            />

            <TextField
              label="Destination Latitude"
              fullWidth
              margin="normal"
              value={destLat}
              onChange={(e) => setDestLat(e.target.value)}
              required
            />

            <TextField
              label="Destination Longitude"
              fullWidth
              margin="normal"
              value={destLng}
              onChange={(e) => setDestLng(e.target.value)}
              required
            />

            <TextField
              select
              label="Vehicle Type"
              fullWidth
              margin="normal"
              value={vehicleType}
              onChange={(e) => setVehicleType(e.target.value)}
            >
              <MenuItem value={1}>Standard</MenuItem>
              <MenuItem value={2}>Premium</MenuItem>
            </TextField>

            <Button
              type="submit"
              variant="contained"
              fullWidth
              sx={{ mt: 2 }}
            >
              Submit Trip
            </Button>
          </Box>
        </Box>
      </Modal>
    </Container>
  );
}
