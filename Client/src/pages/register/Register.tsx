import { useState } from "react";
import "./Register.css";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const Register = () => {
  const [formData, setFormData] = useState({
    username: "",
    email: "",
    password: "",
  });
  const [confirmPassword, setConfirmPassword] = useState<string>("");
  const [passwordMatch, setPasswordMatch] = useState<boolean>(true);

  const navigate = useNavigate();

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (!passwordMatch) return alert("Passwords do not match"); // Extra safety check

    try {
      console.log(formData);
      await axios.post("https://localhost:7018/api/Auth/register", formData);
      alert("Registered successfully!");
      navigate("/login");
    } catch (err: any) {
      console.error(err);
      alert(err?.response?.data?.message || "Failed to register.");
    }
  };

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setFormData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleConfirmPasswordChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const { value } = event.target;
    setConfirmPassword(value);
    setPasswordMatch(formData.password === value);
  };

  return (
    <div className="register-container">
      <h1 className="register-container-title">Register</h1>
      <form className="register-form" onSubmit={handleSubmit}>
        <div className="register-form-group">
          <input
            type="text"
            id="name"
            name="username"
            placeholder="Username"
            value={formData.username}
            onChange={handleChange}
            required
            autoFocus
          />
        </div>
        <div className="separator" />
        <div className="register-form-group">
          <input
            type="email"
            id="email"
            name="email"
            placeholder="E-mail"
            value={formData.email}
            onChange={handleChange}
            required
          />
        </div>
        <div className="separator" />
        <div className="register-form-group">
          <input
            type="password"
            id="password"
            name="password"
            placeholder="Password"
            value={formData.password}
            onChange={handleChange}
            required
          />
          <input
            type="password"
            id="confirm-password"
            name="confirm-password"
            placeholder="Confirm Password"
            value={confirmPassword}
            onChange={handleConfirmPasswordChange}
            required
          />
          {!passwordMatch && (
            <div className="error-message">
              <strong>Passwords do not match</strong>
            </div>
          )}
        </div>
        <div className="register-form-group">
          <a href="/login" className="login-link">
            Already have an account? Login
          </a>
        </div>
        <button
          type="submit"
          id="register-submit-btn"
          disabled={!passwordMatch}
        >
          Sign Up
        </button>
      </form>
    </div>
  );
};

export default Register;
