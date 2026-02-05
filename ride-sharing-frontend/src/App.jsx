import { Routes, Route } from "react-router-dom";
import SignIn from "./pages/SignIn";
import DriverDashboard from "./pages/DriverDashboard";
import RiderDashboard from "./pages/RiderDashboard";

function App() {
  return (
    <Routes>
      <Route path="/login" element={<SignIn />} />
      <Route path="/driver" element={<DriverDashboard />} />
      <Route path="/rider" element={<RiderDashboard />} />
    </Routes>
  );
}

export default App;

