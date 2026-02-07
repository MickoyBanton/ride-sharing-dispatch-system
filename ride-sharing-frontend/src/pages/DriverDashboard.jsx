import React, { useState } from "react";
import axios from "axios";
import {
  Container,
  Typography,
  Button,
  Box,
  Switch,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
} from "@mui/material";

export default function DriverDashboard() {
  const [isOnline, setIsOnline] = useState(false);
  const [openLocationModal, setOpenLocationModal] = useState(false);
  const [latitude, setLatitude] = useState("");
  const [longitude, setLongitude] = useState("");

  const token = localStorage.getItem("token");

  // 🔹 Toggle Online / Offline
  const handleStatusChange = async () => {
    try {
      await axios.put(
        "https://localhost:7044/api/drivers/status",
        { isOnline: !isOnline },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      setIsOnline(!isOnline);
    } catch (error) {
      console.error("Error updating status", error);
      alert("Failed to update driver status");
    }
  };

  // 🔹 Update Location
  const handleUpdateLocation = async () => {
    try {
      await axios.put(
        "https://localhost:7044/api/drivers/location",
        {
          latitude: parseFloat(latitude),
          longitude: parseFloat(longitude),
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      setOpenLocationModal(false);
      alert("Location updated!");
    } catch (error) {
      console.error("Error updating location", error);
      alert("Failed to update location");
    }
  };

  return (
    <Container maxWidth="md">
      <Box mt={5}>
        <Typography variant="h4" gutterBottom>
          Driver Dashboard
        </Typography>

        {/* Online / Offline Section */}
        <Box display="flex" alignItems="center" gap={2} mt={3}>
          <Typography>
            Status: {isOnline ? "Online 🟢" : "Offline 🔴"}
          </Typography>

          <Switch
            checked={isOnline}
            onChange={handleStatusChange}
            color="primary"
          />
        </Box>

        {/* Update Location Button */}
        <Box mt={4}>
          <Button
            variant="contained"
            onClick={() => setOpenLocationModal(true)}
          >
            Update Location
          </Button>
        </Box>
      </Box>

      {/* Location Popup */}
      <Dialog
        open={openLocationModal}
        onClose={() => setOpenLocationModal(false)}
      >
        <DialogTitle>Update Location</DialogTitle>
        <DialogContent>
          <TextField
            label="Latitude"
            fullWidth
            margin="normal"
            value={latitude}
            onChange={(e) => setLatitude(e.target.value)}
          />

          <TextField
            label="Longitude"
            fullWidth
            margin="normal"
            value={longitude}
            onChange={(e) => setLongitude(e.target.value)}
          />
        </DialogContent>

        <DialogActions>
          <Button onClick={() => setOpenLocationModal(false)}>
            Cancel
          </Button>
          <Button variant="contained" onClick={handleUpdateLocation}>
            Save
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
}

