import { useNavigate } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import "./Login.css";
import React, { useState } from "react";

function Login() {
  const { login } = useAuth();
  const navigate = useNavigate();

  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(false);

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    const formData = new FormData(e.currentTarget);
    const email = formData.get("email") as string;
    const password = formData.get("password") as string;

    try {
      await login(email, password);
      navigate("/");
    } catch (error) {
      console.error("Login failed:", error);
      setError("Invalid credentials.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-container">
      <h1 className="login-container-title">Sign In</h1>
      <form className="login-form" onSubmit={handleSubmit}>
        <div className="login-form-group">
          <input
            className="login-input"
            type="email"
            id="email"
            name="email"
            placeholder="E-mail"
            required
            autoFocus={true}
          />
        </div>
        <div className="separator" />
        <div className="login-form-group">
          <input
            className="login-input"
            type="password"
            id="password"
            name="password"
            placeholder="Password"
            required
          />
        </div>
        {error && <p className="login-error">{error}</p>}
        <div className="login-form-group forgot-password-group">
          <a href="#" id="forgot-password-link">
            I forgot my password
          </a>
          <a href="/register" id="register-link">
            Sign up
          </a>
        </div>
        <button type="submit" id="login-submit-btn" disabled={loading}>
          {loading ? "Logging in..." : "Login"}
        </button>
      </form>
    </div>
  );
}

export default Login;
