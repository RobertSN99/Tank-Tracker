import axios from "axios";
import { useState } from "react";
import BackButton from "../../../components/backButton/BackButton";

function CreateClass() {
  const [tankClass, setTankClass] = useState({ name: "" });
  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    try {
      await axios.post("https://localhost:7018/api/tankclass", tankClass, {
        withCredentials: true,
      });
      alert("Class created successfully!");
    } catch (err: any) {
      console.error(err);
      err?.response?.status == 401 && (window.location.href = "/unauthorized");
      alert(err.response?.data?.message || "Failed to create class.");
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setTankClass((prev) => ({
      ...prev,
      [name]: value,
    }));
  };
  return (
    <div className="create-container">
      <h1>Create new Class</h1>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          name="name"
          placeholder="Class Name"
          onChange={handleChange}
          required
        />
        <button className="creation-btn" type="submit">
          Create Class
        </button>
      </form>
      <BackButton />
    </div>
  );
}

export default CreateClass;
