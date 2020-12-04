<?php

namespace App\Http\Requests\Setting;

use Illuminate\Foundation\Http\FormRequest;

class StoreRequest extends FormRequest
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
        if ($this->get('type') == 'images') {

            if ($this->get('image') == 'icon') {
                return [
                    'file' => 'image|max:1000|dimensions:ratio=1/1|dimensions:max_width=512,max_height=512',
                ];
            } else {
                if ($this->get('image') == 'background') {
                    return [
                        'file' => 'image|max:1000|dimensions:max_width=3000,max_height=2500',
                    ];
                }
            }
        }
        return [];
    }

    /**
     * Get custom messages for validator errors.
     *
     * @return array
     */
    public function messages()
    {
        return [
            'file.image' => __('The file has to be an image'),
            'file.max' => __('The file size exceeds :size', ['size' => '1MB']),
            'file.dimensions' => __('The file dimensions are invalid')
        ];
    }
}
