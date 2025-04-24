import axios from "axios";
import { useState } from "react";
import BackButton from "../../../components/backButton/BackButton";

function CreateStatus() {
  const [status, setStatus] = useState({ name: "" });
  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    try {
      await axios.post("https://localhost:7018/api/status", status, {
        withCredentials: true,
      });
      alert("Status created successfully!");
    } catch (err: any) {
      console.error(err);
      err?.response?.status == 401 && (window.location.href = "/unauthorized");
      alert(err.response?.data?.message || "Failed to create status.");
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setStatus((prev) => ({
      ...prev,
      [name]: value,
    }));
  };
  return (
    <div className="create-container">
      <h1>Create new Status</h1>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          name="name"
          placeholder="Status Name"
          onChange={handleChange}
          required
        />
        <button className="creation-btn" type="submit">
          Create Status
        </button>
      </form>
      <BackButton />
    </div>
  );
}

export default CreateStatus;
