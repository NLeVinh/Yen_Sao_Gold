'use client';

import { useState } from 'react';
import { Controller, Control, FieldErrors } from 'react-hook-form';
import { TextField, TextFieldProps, InputAdornment, IconButton } from '@mui/material';
import { Visibility, VisibilityOff } from '@mui/icons-material';

type FormInputProps = {
    name: string;
    label: string;
    control: Control<any>;
    errors: FieldErrors;
    type?: string;
    fullWidth?: boolean;
    variant?: 'outlined' | 'filled' | 'standard';
    placeholder?: string;
} & Partial<Pick<TextFieldProps, 'error' | 'helperText'>>;

export default function FormInput({
    name,
    label,
    control,
    errors,
    type = 'text',
    fullWidth = true,
    variant = 'outlined',
    error,
    helperText,
    ...rest
}: FormInputProps) {
    const [showPassword, setShowPassword] = useState(false);
    const isPasswordField = type === 'password';

    return (
        <Controller
            name={name}
            control={control}
            defaultValue=""
            render={({ field }) => (
                <div className="form-input">
                    <label htmlFor={name} className="form-input__label">
                        {label}
                    </label>
                    <TextField
                        {...field}
                        {...rest}
                        id={name}
                        type={isPasswordField ? (showPassword ? 'text' : 'password') : type}
                        fullWidth={fullWidth}
                        variant={variant}
                        error={!!error}
                        helperText={helperText}
                        label=""
                        placeholder={rest.placeholder || label}
                        InputProps={{
                            style: {
                                padding: '10px',
                            },
                            endAdornment: isPasswordField && (
                                <InputAdornment position="end">
                                    <IconButton
                                        onClick={() => setShowPassword((prev) => !prev)}
                                        edge="end"
                                    >
                                        {showPassword ? <VisibilityOff /> : <Visibility />}
                                    </IconButton>
                                </InputAdornment>
                            ),
                        }}
                    />
                </div>
            )}
        />
    );
}
