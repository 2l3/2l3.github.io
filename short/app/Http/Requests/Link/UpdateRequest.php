<?php

namespace App\Http\Requests\Link;

use Illuminate\Foundation\Http\FormRequest;

class UpdateRequest extends FormRequest
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
            'type' => ['integer'],
            'title' => ['max:255'],
            'archived' => ['boolean'],
            'disabled' => ['boolean'],
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
            'type.integer' => __('The type must be an integer'),

            'title.max' => __('The title may not be greater than :max characters', ['max' => 255]),

            'archived.boolean' => __('The archived field must be true or false'),

            'disabled.boolean' => __('The archived field must be true or false'),
        ];
    }
}
