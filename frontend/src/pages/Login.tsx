import React, { SyntheticEvent, useState } from "react";
import { useNavigate } from "react-router-dom";

const Login = (props: { setName: (name: string) => void }) => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    // Xử lý submit form đăng nhập
    const submit = async (e: SyntheticEvent) => {
        e.preventDefault();

        try {
            const response = await fetch('https://localhost:7242/api/Auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                credentials: 'include',
                body: JSON.stringify({
                    email, password
                })
            });

            if (!response.ok) {
                throw new Error("Email hoặc mật khẩu không đúng");
            }

            // Lấy thông tin người dùng sau khi đăng nhập thành công
            const userResponse = await fetch('https://localhost:7242/api/Auth/user', {
                headers: {
                    'Content-Type': 'application/json'
                },
                credentials: 'include',
            });

            if (userResponse.ok) {
                const userContent = await userResponse.json();
                props.setName(userContent.name); // Cập nhật tên người dùng từ phản hồi của server
            }

            navigate('/'); // Chuyển hướng đến trang chủ sau khi đăng nhập thành công
        } catch (error) {
            console.error('Error:', error); 
            setError('Email hoặc mật khẩu không đúng');
        }
    };

    return (
        <form onSubmit={submit}>
            <h1 className="h3 mb-3 fw-normal">Please sign in</h1>

            <input type="email" className="form-control" placeholder="Email address" required
                value={email} onChange={e => setEmail(e.target.value)} 
            />

            <input type="password" className="form-control" placeholder="Password" required
                value={password} onChange={e => setPassword(e.target.value)}
            />

            {error && <p className="text-danger">{error}</p>}

            <button className="btn btn-primary w-100 py-2" type="submit">Sign in</button>
        </form>
    );
};

export default Login;
