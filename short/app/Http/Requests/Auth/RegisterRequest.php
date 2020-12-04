<?php

namespace App\Http\Requests\Auth;

use Illuminate\Foundation\Http\FormRequest;

class RegisterRequest extends FormRequest
{
    /**
     * Determine if the user is authorized to make this request.
     *
     * @return bool
     */
    public function authorize()
    {
        return true;
    }

    /**
     * Get the validation rules that apply to the request.
     *
     * @return array
     */
    public function rules()
    {
        return [
            'name' => 'required|max:255',
            'email' => 'required|email|max:255|unique:users',
            'password' => 'required|min:6|confirmed',
        ];
    }

    /**
     * Get custom messages for validator errors.
     *
     * @return array
     */
    public function messages()
    {
        return [
            'name.required' => __('The name field is required'),
            'name.max' => __('The name may not be greater than :max characters', ['max' => 255]),
            'email.required' => __('The name field is required'),
            'email.email' => __('The email must be a valid email address'),
            'email.max' => __('The email may not be greater than :max characters', ['max' => 255]),
            'email.unique' => __('The email has already been taken'),
            'password.required' => __('The password field is required'),
            'password.min' => __('The password must be at least :min characters', ['min' => 6]),
            'password.confirmed' => __('The password confirmation does not match'),
        ];
    }
}
