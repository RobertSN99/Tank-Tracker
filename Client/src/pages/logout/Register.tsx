import { useState } from "react";
import "./Register.css";

const Register = () => {
  const [password, setPassword] = useState<string>("");
  const [confirmPassword, setConfirmPassword] = useState<string>("");
  const [passwordMatch, setPasswordMatch] = useState<boolean>(true);

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (!passwordMatch) return alert("Passwords do not match"); // Extra safety check

    const formData = new FormData(event.currentTarget);
    const email = formData.get("email") as string;
    const username = formData.get("name") as string;

    console.log({ username, email, password });
  };

  const handlePasswordChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setPassword(event.target.value);
  };

  const handleConfirmPasswordChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setConfirmPassword(event.target.value);
    setPasswordMatch(password === event.target.value);
  };

  return (
    <div className="register-container">
      <h1 className="register-container-title">Register</h1>
      <form className="register-form" onSubmit={handleSubmit}>
        <div className="register-form-group">
          <input
            type="text"
            id="name"
            name="name"
            placeholder="Username"
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
            value={password}
            onChange={handlePasswordChange}
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
